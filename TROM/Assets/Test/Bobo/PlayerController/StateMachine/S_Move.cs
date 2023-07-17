using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Move : IState
    {
        private Rigidbody2D TargetRb2D => sm.targetRb2D;

            
        [ShowInInspector] private bool isStopping; // Player have no input
        [ShowInInspector] private float timeAfterStop;
        [ShowInInspector] private float timeAfterStart;
        [ShowInInspector] private bool isPositive;
        [ShowInInspector] private bool isFalling;
        [ShowInInspector] private bool started;
        [ShowInInspector] private bool smoothStart;
        [ShowInInspector] private float speedWhenStop;
        [ShowInInspector] private float speedOnEnter;
        
        
        public override void StateEnter()
        {
            sm.targetRb2D.gravityScale = 0;
            started = false;
            timeAfterStart = 0;
            isFalling = false;
            smoothStart = sm.MoveValue == Vector2.zero;
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if (sm.MoveValue == Vector2.zero)
            {
                isStopping = true;
                timeAfterStop = 0;
                speedWhenStop = TargetRb2D.velocity.x;
            }
            else
            {
                isStopping = false;
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
            Vector2 newVelocity = curVelocity;

            if (!sm.detection.grounded && !isFalling)
            {
                isFalling = true;
                sm.targetRb2D.gravityScale = sm.fallGravityScale;
            }
            
            if (isStopping) // Simulate Fiction
            {
                var speed = sm.fakeFiction.Evaluate(timeAfterStop) * speedWhenStop;
                
                if (isPositive ? (speed <= 0) : (speed >= 0))
                {
                    newVelocity.x = 0;
                    sm.Switch(FSM.PlayerState.Idle);
                }
                else
                {
                    newVelocity = TargetRb2D.velocity = new Vector2(speed, TargetRb2D.velocity.y);
                }
                
                timeAfterStop += Time.fixedDeltaTime;
            }
            else
            {
                if (smoothStart)
                {
                    newVelocity.x = sm.MoveValue.x * sm.moveSpeed * sm.smoothStartMoveCurve.Evaluate(timeAfterStart);
                }
                else
                {
                    newVelocity.x = sm.MoveValue.x * sm.moveSpeed;
                }
                
                isPositive = sm.MoveValue.x > 0;
            }

            TargetRb2D.velocity = newVelocity;
        }
    }
}
