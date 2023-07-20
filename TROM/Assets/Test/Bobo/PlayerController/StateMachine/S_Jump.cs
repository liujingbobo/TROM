using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Jump : IState
    {
        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private bool CanCheckGround => timeAfterJump >= sm.jumpGroundCheckGap;

        private bool CanHung
        {
            get
            {
                if (MoveValue.x != 0)
                {
                    if (MoveValue.x > 0)
                    {
                        return sm.detection.frontHungDetector.collider2Ds.Count > 0;
                    }
                    else
                    {
                        return sm.detection.backHungDetector.collider2Ds.Count > 0;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        
        private Vector2 MoveValue => sm.MoveValue;

        private bool jumped;
        private float timeAfterJump;
        private bool isReleased; // Jump button is definitely down when enter

        private GravityType curGravity;

        private bool reachedHighes = false;

        public override void StateEnter()
        {
            jumped = false;
            isReleased = false;
            reachedHighes = false;
        }

        public override void OnJump(InputAction.CallbackContext context)
        {
            if (!isReleased && context.canceled)
            {
                isReleased = true;
            }
        }

        public override void StateFixedUpdate()
        {
            if (jumped == false)
            {
                timeAfterJump = 0;
                TargetRb2D.velocity = new Vector2(sm.MoveValue.x * sm.moveSpeedOnAir, sm.jumpSpeed);
                TargetRb2D.gravityScale = sm.ascendingGravityScaleLight;
                jumped = true;
                sm.PlayAnim(FSM.AnimationType.JumpRise);
            }
            else
            {
                timeAfterJump += Time.fixedDeltaTime;

                var curVelocity = TargetRb2D.velocity;
                var curY = curVelocity.y;
                var curX = curVelocity.x;

                // TODO: Horizontal
                var speedChangeValue = MoveValue.x * sm.airAcceleration * Time.fixedDeltaTime;
                
                if (MoveValue != Vector2.zero)
                {
                    // Move
                    curX += speedChangeValue;
                    curX = Mathf.Clamp(curX, -sm.moveSpeedOnAir,
                        sm.moveSpeedOnAir);
                }
                else
                {
                    // Stop or deceleration\
                    if (curX != 0)
                    {
                        if (curX > 0)
                        {
                            curX -= sm.airAcceleration * Time.fixedDeltaTime;
                            curX = Mathf.Max(curX, 0);
                        }
                        else
                        {                            
                            curX += sm.airAcceleration * Time.fixedDeltaTime;
                            curX = Mathf.Min(curX, 0);
                        }
                    }
                }

                TargetRb2D.velocity = new Vector2(curX, curY);

                // Vertical Gravity
                if (curY < 0) // Falling
                {
                    if (!reachedHighes)
                    {
                        reachedHighes = true;
                        Debug.Log($"Reach Highest: {sm.transform.position.y}");
                    }
                    TargetRb2D.gravityScale = sm.fallGravityScale;
                    sm.PlayAnim(FSM.AnimationType.JumpFall);
                }
                else
                {
                    if (timeAfterJump >= sm.bigJumpTime)
                    {
                        TargetRb2D.gravityScale = sm.ascendingGravityScaleHeavy;
                        sm.PlayAnim(FSM.AnimationType.JumpMid);
                    }
                    else if (timeAfterJump >= sm.smallJumpTime)
                    {
                        if (isReleased)
                        {
                            TargetRb2D.gravityScale = sm.ascendingGravityScaleHeavy;
                            sm.PlayAnim(FSM.AnimationType.JumpMid);
                        }
                    }
                }
                
                // Set sprite position
                if (curX != 0)
                {
                    sm.spriteRenderer.flipX = curX < 0;
                }
                
                // Check Hungable
                // if (curVelocity.y <= 0 && CanHung && !sm.detection.grounded)
                if (curVelocity.y <= 0 && CanHung && !sm.detection.grounded)
                {
                    sm.Switch(FSM.PlayerState.Hang);
                    return;
                }
                
                if (CanCheckGround && sm.detection.grounded && curVelocity.y <= 0)
                {
                    if (sm.MoveValue.x != 0)
                    {
                        Debug.Log($"Jump use time :{timeAfterJump}");
                        sm.FixPosition();
                        sm.Switch(FSM.PlayerState.Move);
                    }
                    else
                    {
                        Debug.Log($"Jump use time :{timeAfterJump}");
                        sm.FixPosition();
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                }


            }
        }

        public override void StateExit()
        {
            TargetRb2D.gravityScale = sm.ascendingGravityScaleLight;
        }

        void SwitchGravity(GravityType t)
        {
            if (!Equals(t, curGravity))
            {
                curGravity = t;
                Debug.Log($"Switch To Gravity {curGravity.ToString()}");
            }

            TargetRb2D.gravityScale = curGravity switch
            {
                GravityType.Light => sm.ascendingGravityScaleLight,
                GravityType.Heavy => sm.ascendingGravityScaleHeavy,
                GravityType.Fall => sm.fallGravityScale,
                _ => sm.ascendingGravityScaleLight
            };
        }
        
        enum GravityType
        {
            Light, Heavy, Fall
        }
    }
}