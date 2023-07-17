using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Move : IState
    {
        private Rigidbody2D Rigidbody2D => sm.rigidbody2D;

        public override void StateEnter()
        {
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if(sm.MoveValue == Vector2.zero) sm.Switch(FSM.PlayerState.Idle);
            
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
            if(Rigidbody2D) sm.rigidbody2D.velocity = new Vector2(sm.MoveValue.x * sm.moveSpeed, 0);
        }
    }
}
