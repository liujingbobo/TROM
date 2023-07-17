using System;
using System.Collections;
using System.Collections.Generic;
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
        public GameObject character;
        public PlayerDetection detection;
        public PlayerController controller;

        public AnimationCurve smoothStartMoveCurve;
        public AnimationCurve fakeFiction;
        public AnimationCurve initJumpGravityCurve;
        public float regularGravityScale = 1.0f;
        public float fallGravityScale = 1.5f;
        public float moveSpeed = 10f;
        public float jumpSpeed = 10;
        public float moveSpeedOnAir;
        public float groundCheckGap;
        public float coyoteTime;
        
        private Vector2 moveValue;

        public Vector2 MoveValue => moveValue;
        
        public Rigidbody2D targetRb2D;

        private readonly Dictionary<PlayerState, IState> stateMachine = new Dictionary<PlayerState, IState>();

        [ShowInInspector] private PlayerState curStateTag = PlayerState.InValid;
        [ShowInInspector] private PlayerState preStateTag = PlayerState.InValid;

        public PlayerState PreStateTag => preStateTag;
        
        private IState CurState => curStateTag != PlayerState.InValid && stateMachine.ContainsKey(curStateTag)
            ? stateMachine[curStateTag]
            : null;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            stateMachine[PlayerState.Idle] = new S_Idle().Init(this);
            stateMachine[PlayerState.Move] = new S_Move().Init(this);
            stateMachine[PlayerState.Jump] = new S_Jump().Init(this);
            stateMachine[PlayerState.Falling] = new S_Falling().Init(this);
            Switch(PlayerState.Idle);
        }

        public void Switch(PlayerState targetState)
        {
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

        #region Actions

        public void OnMove(InputAction.CallbackContext context)
        {
            moveValue = context.ReadValue<Vector2>();
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
        }
    }
}

