using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Move : IState
    {
        private Rigidbody2D Rigidbody2D => sm.rigidbody2D;

        private Vector2 _speed = Vector2.zero;
        public override void StateEnter()
        {
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            _speed = context.ReadValue<Vector2>();
            Debug.Log(_speed);

            if (_speed != Vector2.zero) return;
            
            sm.Switch(FSM.PlayerState.Idle);
        }

        public override void OnJump(InputAction.CallbackContext context)
        {
            
            if (context.started)
            {
                sm.Switch(FSM.PlayerState.Jump);
            }
        }

        public override void StateFixedUpdate()
        {
            if(Rigidbody2D) sm.rigidbody2D.velocity = new Vector2(_speed.x * sm.moveSpeed, 0);
        }
    }
}
