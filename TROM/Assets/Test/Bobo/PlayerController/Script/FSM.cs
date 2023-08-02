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

        [BoxGroup("Other")]
        public Rigidbody2D targetRb2D;

        private readonly Dictionary<PlayerState, IState> stateMachine = new Dictionary<PlayerState, IState>();

        public IState JumpState;
        public IState IdleState;
        public IState MoveState;
        public IState HangState;
        public IState FallingState;
        public IState LadderState;
        public IState AttackState;

        [ShowInInspector] private PlayerState curStateTag = PlayerState.InValid;
        [ShowInInspector] private PlayerState preStateTag = PlayerState.InValid;
        
        public Vector2 MoveValue { get; private set; }
        public Vector2 posDirection;
        
        public PlayerState PreStateTag => preStateTag;
        public AnimationType CurAnimation = AnimationType.Empty;

        private IState CurState => curStateTag != PlayerState.InValid && stateMachine.ContainsKey(curStateTag)
            ? stateMachine[curStateTag]
            : null;

        private void Start()
        {
            stateMachine[PlayerState.Jump] = JumpState.Init(this);
            stateMachine[PlayerState.Idle] = IdleState.Init(this);
            stateMachine[PlayerState.Move] = MoveState.Init(this);
            stateMachine[PlayerState.Hang] = HangState.Init(this);
            stateMachine[PlayerState.Fall] = FallingState.Init(this);
            stateMachine[PlayerState.Ladder] = LadderState.Init(this);
            stateMachine[PlayerState.Attack] = AttackState.Init(this);

            Switch(PlayerState.Idle);
        }

        public void Switch(PlayerState targetState)
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
                value1.StateEnter(preStateTag);
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
                AnimationType.Attack => "SwordAttack"
            });
        }

        public void FixPosition()
        {
            detection.ResetGrounded();
            
            if (detection.IsGrounded)
            {
                if (detection.GroundHit.collider != null)
                {
                    var newPosition = transform.position;

                    var hit = detection.GroundHit;
                    print($"Fix Position to {new Vector3(hit.point.x, hit.point.y, newPosition.z)}");
                    newPosition = new Vector3(hit.point.x, hit.point.y, newPosition.z);
                    transform.position = newPosition;
                }
            }
        }

        public void SetDirection(PlayerDirection dir)
        {
            if (dir == direction) return;
            
            direction = dir;
            spriteRenderer.flipX = dir == PlayerDirection.Back;
        }
        
        #region Event

        private void Update()
        {
            if (curStateTag != PlayerState.InValid && stateMachine.ContainsKey(curStateTag))
            {
                stateMachine[curStateTag].StateUpdate();
            }
        }
        private void LateUpdate()
        {
            CurState?.StateLateUpdate();
        }
        
        private void FixedUpdate()
        {
            detection.ResetGrounded();

            var hit = detection.GroundHit;

            if (detection.IsGrounded && hit.collider != null)
            {
                var normal = hit.normal;
                
                posDirection = Vector3.ProjectOnPlane(Vector2.right, normal).normalized;
            }
            
            CurState?.StateFixedUpdate();
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
        
        #endregion


    }
}

