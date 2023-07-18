using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Move : IState
    {
        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private Vector2 MoveValue => sm.MoveValue;
            
        [ShowInInspector] private float timeAfterStop;
        [ShowInInspector] private float timeAfterStart;
        [ShowInInspector] private bool isFalling;
        [ShowInInspector] private bool started;

        public override void StateEnter()
        {
            started = false;
            timeAfterStart = 0;
            isFalling = false;
            sm.PlayAnim(FSM.AnimationType.Run);
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if (sm.MoveValue == Vector2.zero)
            {
                timeAfterStop = 0;
            }
            else
            {
                timeAfterStop = 0;
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
            if (!started)
            {
                started = true;
            }
            else
            {
                timeAfterStart += Time.fixedDeltaTime;
            }
            
            var curVelocity = sm.targetRb2D.velocity;

            // TODO: Falling
            if (!sm.detection.grounded)
            {
                if(!isFalling)
                {
                    isFalling = true;
                }
                else
                {
                    sm.Switch(FSM.PlayerState.Falling);
                    return;
                }
            }
            else
            {
                isFalling = false;
            }
            
            var speedChangeValue = MoveValue.x * sm.acceleration * Time.fixedDeltaTime;
            var curX = curVelocity.x;
            if (MoveValue != Vector2.zero)
            {
                // Move
                curX += speedChangeValue;
                curX = Mathf.Clamp(curX, -sm.moveSpeed,
                    sm.moveSpeed);
            }
            else
            {
                // Stop or deceleration\
                if (curX != 0)
                {
                    if (curX > 0)
                    {
                        curX -= sm.acceleration * Time.fixedDeltaTime;
                        curX = Mathf.Max(curX, 0);
                    }
                    else
                    {                            
                        curX += sm.acceleration * Time.fixedDeltaTime;
                        curX = Mathf.Min(curX, 0);
                    }

                    if (curX == 0)
                    {
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                }
            }

            TargetRb2D.velocity = new Vector2(curX, 0);
            
            if (curX != 0)
            {
                sm.spriteRenderer.flipX = curX < 0;
            }
        }
    }
}
