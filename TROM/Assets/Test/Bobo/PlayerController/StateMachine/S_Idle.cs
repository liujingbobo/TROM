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
            sm.targetRb2D.gravityScale = 0;
            sm.targetRb2D.velocity = Vector2.zero;
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if (sm.MoveValue != Vector2.zero)
            {
                sm.Switch(FSM.PlayerState.Move);
            }
        }

        public override void OnJump(InputAction.CallbackContext context)
        {
            if (context is { started: true, canceled: false })
            {
                sm.Switch(FSM.PlayerState.Jump);
            }
        }

        public override void StateFixedUpdate()
        {
            if (!sm.detection.grounded)
            {
                sm.Switch(FSM.PlayerState.Falling);
            }
        }
    }
}
