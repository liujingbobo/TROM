using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Jump : IState
    {
        // Jump
        [BoxGroup("Jump")] public float jumpSpeed = 10;
        [BoxGroup("Jump")] public float jumpGroundCheckGap = 0.1f; 
        [BoxGroup("Jump")] public float ascendingGravityScaleLight = 1.0f;
        [BoxGroup("Jump")] public float ascendingGravityScaleHeavy = 1.0f;
        [BoxGroup("Jump")] public float fallGravityScale = 1.5f;
        [BoxGroup("Jump")] public float smallJumpTime;
        [BoxGroup("Jump")] public float bigJumpTime;
        [BoxGroup("Jump")] public float airAcceleration; 
        [BoxGroup("Jump")] public float moveSpeedOnAir; // Separate the speed on air and ground
        [BoxGroup("Jump")] public float horizontalMoveThreshold; // Only change velocity when moveValue.x > horizontalMoveThreshold
        
        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private bool CanCheckGround => timeAfterJump >= jumpGroundCheckGap;
        private bool CanHang
        {
            get
            {
                if (MoveValue.x != 0)
                {
                    return sm.detection.hangDetector.collider2Ds.Count > 0;
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

        private JumpState curState;

        public override void StateEnter(FSM.PlayerState preState)
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
            var curVelocity = TargetRb2D.velocity;
            var curVelocityY = curVelocity.y;
            var curVelocityX = curVelocity.x;
            var inputX = MoveValue.x;
            var inputY = MoveValue.y;
            if (curState == JumpState.Boost)
            {
                timeAfterJump = 0;
                TargetRb2D.velocity = new Vector2(sm.MoveValue.x * moveSpeedOnAir, jumpSpeed);
                TargetRb2D.gravityScale = ascendingGravityScaleLight;
                sm.PlayAnim(FSM.AnimationType.JumpRise);
                curState = JumpState.Rise;
                return;
            }
            else
            {
                // Jump time so far
                timeAfterJump += Time.fixedDeltaTime;

                #region HorizontalMove

                // Check if the current input is enough for changing the speed. 
                bool horizontalInputValid = Mathf.Abs(inputX) >= horizontalMoveThreshold;
                
                if (horizontalInputValid)
                {
                    var horizontalSpeedChangeValue = (inputX > 0 ? 1 : -1) * airAcceleration * Time.fixedDeltaTime;

                    // Move
                    curVelocityX += horizontalSpeedChangeValue;
                    curVelocityX = Mathf.Clamp(curVelocityX, -moveSpeedOnAir,
                        moveSpeedOnAir);
                }
                else
                {
                    // Stop or deceleration\
                    if (curVelocityX != 0)
                    {
                        if (curVelocityX > 0)
                        {
                            curVelocityX -= airAcceleration * Time.fixedDeltaTime;
                            curVelocityX = Mathf.Max(curVelocityX, 0);
                        }
                        else
                        {
                            curVelocityX += airAcceleration * Time.fixedDeltaTime;
                            curVelocityX = Mathf.Min(curVelocityX, 0);
                        }
                    }
                }
                
                TargetRb2D.velocity = new Vector2(curVelocityX, curVelocityY);
                #endregion

                #region VerticalMove

                // Vertical Gravity
                if (curState == JumpState.Rise)
                {
                    if (timeAfterJump >= bigJumpTime)
                    {
                        TargetRb2D.gravityScale = ascendingGravityScaleHeavy;
                        sm.PlayAnim(FSM.AnimationType.JumpMid);
                    }
                    else if (timeAfterJump >= smallJumpTime)
                    {
                        if (isReleased)
                        {
                            TargetRb2D.gravityScale = ascendingGravityScaleHeavy;
                            sm.PlayAnim(FSM.AnimationType.JumpMid);
                        }
                    }

                    if (curVelocityY == 0)
                    {
                        curState = JumpState.Fall;
                        Debug.Log($"Reach Highest: {sm.transform.position.y}");
                        TargetRb2D.gravityScale = fallGravityScale;
                        sm.PlayAnim(FSM.AnimationType.JumpFall);
                    }
                }

                #endregion

                // Set sprite position
                if (curVelocityX != 0)
                {
                    sm.spriteRenderer.flipX = curVelocityX < 0;
                }
                
                #region HangDetection

                // Check Hangable
                // if (curVelocity.y <= 0 && CanHung && !sm.detection.grounded)
                if (CanHang && !sm.detection.grounded)
                {
                    sm.Switch(FSM.PlayerState.Hang);
                    return;
                }
                
                #endregion
                
                #region GroundDetection

                if (curState == JumpState.Fall)
                {
                    if (CanCheckGround && sm.detection.grounded)
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
                
                #endregion
            }
        }

        public override void StateExit()
        {
            TargetRb2D.gravityScale = ascendingGravityScaleLight;
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
                GravityType.Light => ascendingGravityScaleLight,
                GravityType.Heavy => ascendingGravityScaleHeavy,
                GravityType.Fall => fallGravityScale,
                _ => ascendingGravityScaleLight
            };
        }

        enum GravityType
        {
            Light,
            Heavy,
            Fall
        }

        enum JumpState
        {
            Boost,
            Rise,
            Fall
        }
    }
}