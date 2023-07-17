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

            if (sm.MoveValue != Vector2.zero)
            {
                sm.Switch(FSM.PlayerState.Move);
            }
        }
    
        public override void OnMove(InputAction.CallbackContext context)
        {
            sm.Switch(FSM.PlayerState.Move);
        }

        public override void OnJump(InputAction.CallbackContext context)
        {
            if (context is { started: true, canceled: false })
            {
                sm.Switch(FSM.PlayerState.Jump);
            }
        }
        
    }
}
