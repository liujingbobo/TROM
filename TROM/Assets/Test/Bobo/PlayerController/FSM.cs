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

        public PlayerDirection direction;
        
        // Move
        [BoxGroup("Move")]public float moveSpeed = 10f;
        [BoxGroup("Move")]public float coyoteTime;
        [BoxGroup("Move")]public float acceleration;
        [BoxGroup("Move")]public float turnSpeed;

        // Hang
        public float hangSnappingSpeed = 5;
        public float hangSnapRange = 0.5f;
        public Vector2 hangStartOffset;
        public Vector2 hangEndOffset;
        
        [BoxGroup("Other")]
        public Rigidbody2D targetRb2D;

        private readonly Dictionary<PlayerState, IState> stateMachine = new Dictionary<PlayerState, IState>();

        public IState JumpState;
        public IState IdleState;
        public IState MoveState;
        public IState HangState;
        public IState FallingState;

        [ShowInInspector] private PlayerState curStateTag = PlayerState.InValid;
        [ShowInInspector] private PlayerState preStateTag = PlayerState.InValid;
        
        public Vector2 MoveValue { get; private set; }
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
            stateMachine[PlayerState.Falling] = FallingState.Init(this);

            Init();
        }

        private void Init()
        {
            Switch(PlayerState.Idle);
        }

        public void Switch(PlayerState targetState)
        {
            Debug.Log($"Ready to switch to {targetState.ToString()}");
            
            if (curStateTag != PlayerState.InValid)
            {
                if (stateMachine.ContainsKey(curStateTag))
                {
                    stateMachine[curStateTag].StateExit();
                }
            }

            preStateTag = curStateTag;
            
            curStateTag = targetState;

            if (stateMachine.ContainsKey(curStateTag))
            {
                stateMachine[curStateTag].StateEnter();
            }
        }

        public void PlayAnim(AnimationType animationType)
        {
            if (CurAnimation == animationType) return;
            
            animator.Play(animationType switch
            {
                AnimationType.Idle => "Idle",
                AnimationType.Walk => "Walk",
                AnimationType.Run => "Run",
                AnimationType.JumpRise => "JumpRise",
                AnimationType.JumpFall => "JumpFall",
                AnimationType.JumpMid => "JumpMid"
            });
        }

        public void FixPosition()
        {
            var newPosition = transform.position;
                
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(newPosition.x, newPosition.y) + Vector2.up, Vector2.down, 1,
                LayerMask.GetMask($"Ground"));

            if (hit.collider != null)
            {
                newPosition = new Vector3(hit.point.x, hit.point.y, newPosition.z);
                transform.position = newPosition;
            }
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
            CurState?.StateFixedUpdate();
        }

        #endregion


        #region Actions

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<Vector2>();
            CurState?.OnMove(context);
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            CurState?.OnJump(context);
        }
        
        #endregion

        public enum PlayerState
        {
            InValid = -1,
            Idle = 0,
            Move = 10,
            Jump = 20,
            Climb = 30,
            Falling = 40,
            Hang = 50,
        }

        public enum AnimationType
        {
            Empty,
            Idle,
            
            WalkStart,
            Walk,
            WalkEnd,
            
            Run,
            
            JumpRise,
            JumpMid,
            JumpFall,
        }
        public enum PlayerDirection
        {
            Front,
            Back
        }
    }
}

