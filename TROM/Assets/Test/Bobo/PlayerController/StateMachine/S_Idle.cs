using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Idle : IState
    {
        public override void StateEnter()
        {
            // TODO: Play Anim
        }
        public override void StateUpdate()
        {
            // TODO: 
        }
    
        public override void OnMove(InputAction.CallbackContext context)
        {
            sm.Switch(FSM.PlayerState.Move);
        }

        public override void OnJump(InputAction.CallbackContext context)
        {
            sm.Switch(FSM.PlayerState.Jump);
        }
    }
}
