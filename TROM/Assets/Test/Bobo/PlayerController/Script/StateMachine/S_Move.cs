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
        [BoxGroup("Move")]public float maxGapOnGround = 0.01f;

        [BoxGroup("Jump")]public float ladderEnterThresholdOnY = 0.1f;
        [BoxGroup("Jump")]public float moveThresholdOnX = 0.1f;

        public float normalGravity = 1;
        
        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private Vector2 MoveValue => sm.MoveValue;
            
        [ShowInInspector] private float timeAfterStop;
        [ShowInInspector] private float timeAfterStart;
        [ShowInInspector] private bool isFalling;
        [ShowInInspector] private bool started;

        public override void StateEnter(PlayerState preState)
        {
            sm.targetRb2D.gravityScale = normalGravity;
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

            var curDir = sm.targetRb2D.velocity.normalized;
            
            var curVelocity = Mathf.Sign(curDir.x) * sm.targetRb2D.velocity.magnitude;

            // TODO: Falling
            if (!sm.detection.isGrounded)
            {
                if (!sm.detection.isGrounded)
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
            
            if (Mathf.Abs(MoveValue.x) >= moveThresholdOnX)
            {
                // Move
                if (curDir.x * MoveValue.x < 0)
                {
                    // use turn speed
                    speedChangeValue = Mathf.Sign(MoveValue.x) * turnSpeed * Time.fixedDeltaTime;
                }

                curVelocity += speedChangeValue;
                curVelocity = Mathf.Clamp(curVelocity, -moveSpeed,
                    moveSpeed);
            }
            else
            {
                // Stop or deceleration\
                if (curVelocity != 0)
                {
                    if (curVelocity > 0)
                    {
                        curVelocity -= acceleration * Time.fixedDeltaTime;
                        curVelocity = Mathf.Max(curVelocity, 0);
                    }
                    else
                    {                            
                        curVelocity += acceleration * Time.fixedDeltaTime;
                        curVelocity = Mathf.Min(curVelocity, 0);
                    }

                    if (curVelocity == 0)
                    {
                        sm.Switch(PlayerState.Idle);
                    }
                }
                else
                {
                    sm.Switch(PlayerState.Idle);
                }
            }

            TargetRb2D.velocity = sm.posDirection * curVelocity;
            
            if (curVelocity != 0)
            {
                sm.SetDirection(curVelocity < 0 ? PlayerDirection.Back : PlayerDirection.Front);
            }
        }
    }
}
