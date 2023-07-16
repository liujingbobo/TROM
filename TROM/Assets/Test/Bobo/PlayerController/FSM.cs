using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerControllerTest
{
    public class FSM : MonoBehaviour
    {
        public GameObject character;
        public enum PlayerState
        {
            InValid = -1,
            Idle = 0,
            Move = 10,
            Jump = 20,
            Climb = 30
        }
    
        private Dictionary<PlayerState, IState> StateMachine = new Dictionary<PlayerState, IState>();

        [ShowInInspector] private PlayerState _curState = PlayerState.InValid;

        private void Start()
        {
            Init();
        }
        void Init()
        {
            Switch(PlayerState.Idle);
        }
        public void Switch(PlayerState targetState)
        {
            if (_curState != PlayerState.InValid)
            {
                if (StateMachine.ContainsKey(_curState))
                {
                    StateMachine[_curState].StateExit();
                }
            }

            _curState = targetState;
            
            if (StateMachine.ContainsKey(_curState))
            {
                StateMachine[_curState].StateEnter();
            }
        }
        private void Update()
        {
            if (_curState != PlayerState.InValid && StateMachine.ContainsKey(_curState))
            {
                StateMachine[_curState].StateUpdate();
            }
        }
    }
}

