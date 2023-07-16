using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerControllerTest
{
    public class FSM : MonoBehaviour
    {
        public GameObject character;
        public PlayerDetection detection;
        public PlayerController controller;

        public float regularGravity;
        public float fallGravity;
        public float moveSpeed;
        public float jumpSpeed;
        public float moveSpeedOnAir;
        
        public Rigidbody2D rigidbody2D;
        
        public enum PlayerState
        {
            InValid = -1,
            Idle = 0,
            Move = 10,
            Jump = 20,
            Climb = 30
        }

        private Dictionary<PlayerState, IState> StateMachine = new Dictionary<PlayerState, IState>();

        [ShowInInspector] private PlayerState _curStateTag = PlayerState.InValid;

        private IState CurState => _curStateTag != PlayerState.InValid && StateMachine.ContainsKey(_curStateTag)
            ? StateMachine[_curStateTag]
            : null;

        private void Start()
        {
            Init();
        }

        void Init()
        {
            StateMachine[PlayerState.Idle] = new S_Idle().Init(this);
            StateMachine[PlayerState.Move] = new S_Move().Init(this);
            StateMachine[PlayerState.Jump] = new S_Jump().Init(this);
            Switch(PlayerState.Idle);
        }

        public void Switch(PlayerState targetState)
        {
            if (_curStateTag != PlayerState.InValid)
            {
                if (StateMachine.ContainsKey(_curStateTag))
                {
                    print($"State Machine: Exit state {_curStateTag.ToString()}");
                    StateMachine[_curStateTag].StateExit();
                }
            }

            _curStateTag = targetState;

            if (StateMachine.ContainsKey(_curStateTag))
            {
                print($"State Machine: Enter state {_curStateTag.ToString()}");
                StateMachine[_curStateTag].StateEnter();
            }
        }

        private void Update()
        {
            if (_curStateTag != PlayerState.InValid && StateMachine.ContainsKey(_curStateTag))
            {
                StateMachine[_curStateTag].StateUpdate();
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

        public Vector2 MoveValue;
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<Vector2>();
            Debug.Log($"Trigger Move in FSM with value {MoveValue}");
            CurState?.OnMove(context);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log($"Trigger Jump in FSM with.");
            CurState?.OnJump(context);
        }
        
        #endregion

    }
}

