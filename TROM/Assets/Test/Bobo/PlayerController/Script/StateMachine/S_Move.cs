using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Move : IState
    {
                
        // Move
        [BoxGroup("Move")]public float moveSpeed = 10f;
        [BoxGroup("Move")]public float acceleration;
        [BoxGroup("Move")]public float turnSpeed;
        [BoxGroup("Jump")]public float ladderEnterThresholdOnY = 0.1f;


        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private Vector2 MoveValue => sm.MoveValue;
            
        [ShowInInspector] private float timeAfterStop;
        [ShowInInspector] private float timeAfterStart;
        [ShowInInspector] private bool isFalling;
        [ShowInInspector] private bool started;

        public override void StateEnter(PlayerState preState)
        {
            sm.targetRb2D.gravityScale = 0;
            started = false;
            timeAfterStart = 0;
            isFalling = false;
            sm.PlayAnim(AnimationType.Run);
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
                sm.Switch(PlayerState.Jump);
            }
        }
        
        public override void OnAttack(InputAction.CallbackContext context)
        {
            if (context is { started: true, canceled: false })
            {
                sm.Switch(PlayerState.Attack);
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
                    sm.Switch(PlayerState.Fall);
                    return;
                }
            }
            else
            {
                isFalling = false;
            }
            
            if (Mathf.Abs(sm.MoveValue.y) > ladderEnterThresholdOnY)
            {
                if (sm.detection.ladderDetector.collider2Ds.Count > 0)
                {
                    var collider = sm.detection.ladderDetector.collider2Ds.Last();
                    
                    if (collider.GetComponent<LadderInfo>() is { } li)
                    {
                        if (collider == li.bottomCollider)
                        {
                            if (sm.MoveValue.y > 0)
                            {
                                sm.Switch(PlayerState.Ladder);
                                return;
                            }
                        }
                        else
                        {
                            if (sm.MoveValue.y < 0)
                            {
                                sm.Switch(PlayerState.Ladder);
                                return;
                            }
                        }
                    }
                }
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
                        sm.Switch(PlayerState.Idle);
                    }
                }
                else
                {
                    sm.Switch(PlayerState.Idle);
                }
            }

            TargetRb2D.velocity = new Vector2(curX, 0);
            
            if (curX != 0)
            {
                sm.SetDirection(curX < 0 ? PlayerDirection.Back : PlayerDirection.Front);
            }
        }
    }
}
