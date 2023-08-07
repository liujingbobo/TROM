using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerControllerTest
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FSM : MonoBehaviour
    {
        // Components
        public GameObject character;
        public PlayerDetection detection;
        public PlayerController controller;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Collider2D playerCollider;
        public PlayerDirection direction;
        public Interactor2D interactor2D;

        [BoxGroup("Other")] public Rigidbody2D targetRb2D;

        private readonly Dictionary<PlayerState, IState> stateMachine = new Dictionary<PlayerState, IState>();

        [FormerlySerializedAs("JumpState")] public IState jumpState;
        [FormerlySerializedAs("IdleState")] public IState idleState;
        [FormerlySerializedAs("MoveState")] public IState moveState;
        [FormerlySerializedAs("HangState")] public IState hangState;
        [FormerlySerializedAs("FallingState")] public IState fallingState;
        [FormerlySerializedAs("LadderState")] public IState ladderState;
        [FormerlySerializedAs("AttackState")] public IState attackState;
        [FormerlySerializedAs("CheckItemContainer")] public IState checkItemContainerState;
        [FormerlySerializedAs("HitState")] public IState hitState;

        [ShowInInspector] private PlayerState curStateTag = PlayerState.InValid;
        [ShowInInspector] private PlayerState preStateTag = PlayerState.InValid;

        public PlayerState CurrentState => curStateTag;
        public Vector2 MoveValue { get; private set; }

        public (Vector2 posDirection, float stickToGroundY) curSlopeInfo;
        
        public (Vector2 posDirection, float stickToGroundY) GetPosDirection()
        {
            return detection.GetSlopeInfo(MoveValue);
        }
        
        public Vector2 posDirection;
        
        public PlayerState PreStateTag => preStateTag;
        
        public AnimationType CurAnimation = AnimationType.Empty;

        private IState CurState => curStateTag != PlayerState.InValid && stateMachine.ContainsKey(curStateTag)
            ? stateMachine[curStateTag]
            : null;

        private void Start()
        {
            stateMachine[PlayerState.Jump] = jumpState.Init(this);
            stateMachine[PlayerState.Idle] = idleState.Init(this);
            stateMachine[PlayerState.Move] = moveState.Init(this);
            stateMachine[PlayerState.Hang] = hangState.Init(this);
            stateMachine[PlayerState.Fall] = fallingState.Init(this);
            stateMachine[PlayerState.Ladder] = ladderState.Init(this);
            stateMachine[PlayerState.Attack] = attackState.Init(this);
            stateMachine[PlayerState.CheckItemContainer] = checkItemContainerState.Init(this);
            stateMachine[PlayerState.Hit] = hitState.Init(this);

            Switch(PlayerState.Idle);
        }

        public void Switch(PlayerState targetState,params object[] objects)
        {
            Debug.Log($"Ready to switch to {targetState.ToString()}");
            
            if (curStateTag != PlayerState.InValid)
            {
                if (stateMachine.TryGetValue(curStateTag, out var value))
                {
                    value.StateExit();
                }
            }

            preStateTag = curStateTag;
            
            curStateTag = targetState;

            if (stateMachine.TryGetValue(curStateTag, out var value1))
            {
                value1.StateEnter(preStateTag,objects);
            }
        }

        public void PlayAnim(AnimationType animationType)
        {
            if (CurAnimation == animationType)
            {
                print($"Curanimation is the same as target. {CurAnimation.ToString()}");
                return;
            }
            
            animator.Play(animationType switch
            {
                AnimationType.Idle => "Idle",
                AnimationType.Walk => "Walk",
                AnimationType.Run => "Run",
                AnimationType.JumpRise => "JumpRise",
                AnimationType.JumpFall => "JumpFall",
                AnimationType.JumpMid => "JumpMid",
                AnimationType.LedgeHangPreview => "LedgeHangPreview",
                AnimationType.LedgeClimbPreview => "LedgeClimbPreview",
                AnimationType.LedgeClimbPreviewReverse => "LedgeClimbPreviewReverse",
                AnimationType.LadderClimb => "LadderClimb",
                AnimationType.LadderClimbFinish => "LadderClimbFinish",
                AnimationType.LadderClimbReverse => "LadderClimbReverse",
                AnimationType.LadderClimbFinishReverse => "LadderClimbFinishReverse",
                AnimationType.Attack => "Kick03",
                AnimationType.CheckItemContainer => "Kick02",
                AnimationType.Hit => "SwordGuardImpact",
            });
        }

        // fix player position based on previous ground check. 
        public void FixPosition()
        {
            if (!detection.isGrounded) return;
            
            var newPosition = transform.position;
            newPosition = new Vector3(newPosition.x, GetPosDirection().stickToGroundY, newPosition.z);
            targetRb2D.transform.position = newPosition;
        }

        // Do ground check first, then fix player position based on current input. 
        public void ForceFixPosition()
        {
            detection.GroundDetect();

            curSlopeInfo = detection.GetSlopeInfo(MoveValue);

            FixPosition();
        }

        public void SetDirection(PlayerDirection dir)
        {
            if (dir == direction) return;
            
            direction = dir;
            spriteRenderer.flipX = dir == PlayerDirection.Left;
        }
        
        #region UnityEvent

        private void Update()
        {
            if(CurState) CurState.StateUpdate();
        }
        
        private void LateUpdate()
        {
            if(CurState) CurState.StateLateUpdate();
        }
        
        private void FixedUpdate()
        {
            detection.GroundDetect();

            curSlopeInfo = GetPosDirection();
            
            posDirection = detection.isGrounded ? curSlopeInfo.posDirection : Vector2.right;
            
            if(CurState) CurState.StateFixedUpdate();
        }

        #endregion

        #region Actions

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<Vector2>();
            if(CurState != null) CurState.OnMove(context);
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if(CurState != null) CurState
            .OnJump(context);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(CurState != null) CurState.OnAttack(context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (interactor2D.CheckInteractable())
            {
                interactor2D.Interact();
            }
        }
        #endregion
        
        public void GetAttacked(AttackReleaseInfo attackReleaseInfo)
        {
            Switch(PlayerState.Hit, attackReleaseInfo);
        }
        
        public void OpenBackPack(ItemContainer container)
        {
            var targetState = PlayerState.CheckItemContainer;
                
            Debug.Log($"Ready to switch to {targetState.ToString()}");
            
            if (curStateTag != PlayerState.InValid)
            {
                if (stateMachine.TryGetValue(curStateTag, out var value))
                {
                    value.StateExit();
                }
            }

            preStateTag = curStateTag;
            
            curStateTag = targetState;

            if (stateMachine.TryGetValue(curStateTag, out var value1))
            {
                value1.StateEnter(preStateTag, container);
            }
        }

        public void CustomAction()
        {
            
        }
    }
}

