using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class PlayerController : MonoBehaviour
    {
        public FSM StateMachine;

        public void Start()
        {
            StateMachine.Switch(FSM.PlayerState.Idle);
        }
    }
}
