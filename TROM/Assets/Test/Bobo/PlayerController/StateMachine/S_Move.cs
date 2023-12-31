using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Move : IState
    {
                
        // Move
        [BoxGroup("Move")]public float moveSpeed = 10f;
        [BoxGroup("Move")]public float coyoteTime;
        [BoxGroup("Move")]public float acceleration;
        [BoxGroup("Move")]public float turnSpeed;
        

        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private Vector2 MoveValue => sm.MoveValue;
            
        [ShowInInspector] private float timeAfterStop;
        [ShowInInspector] private float timeAfterStart;
        [ShowInInspector] private bool isFalling;
        [ShowInInspector] private bool started;

        public override void StateEnter(FSM.PlayerState preState)
        {
            sm.targetRb2D.gravityScale = 0;
            started = false;
            timeAfterStart = 0;
            isFalling = false;
            sm.PlayAnim(FSM.AnimationType.Run);
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if (sm.MoveValue.x == 0)
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
            
            
            
            var speedChangeValue = MoveValue.x * acceleration * Time.fixedDeltaTime;
            var curX = curVelocity.x;
            
            if (MoveValue.x != 0)
            {
                // Move

                if (curX * MoveValue.x < 0)
                {
                    // use turn speed
                    speedChangeValue = MoveValue.x * turnSpeed * Time.fixedDeltaTime;
                }

                curX += speedChangeValue;
                curX = Mathf.Clamp(curX, -moveSpeed,
                    moveSpeed);
            }
            else
            {
                // Stop or deceleration\
                if (curX != 0)
                {
                    if (curX > 0)
                    {
                        curX -= acceleration * Time.fixedDeltaTime;
                        curX = Mathf.Max(curX, 0);
                    }
                    else
                    {                            
                        curX += acceleration * Time.fixedDeltaTime;
                        curX = Mathf.Min(curX, 0);
                    }

                    if (curX == 0)
                    {
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                }
                else
                {
                    sm.Switch(FSM.PlayerState.Idle);
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
