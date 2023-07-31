using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [BoxGroup("Jump")] public float ladderEnterThresholdOnY = 0.1f;
        
        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        private bool CanCheckGround => timeAfterJump >= jumpGroundCheckGap;
        private bool CanHang
        {
            get
            {
                if (MoveValue.x != 0)
                {
                    return sm.detection.upperHangDetector.collider2Ds.Count > 0;
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

        [ShowInInspector]private JumpState curState;

        public override void StateEnter(PlayerState preState)
        {
            jumped = false;
            isReleased = false;
            reachedHighes = false;
            curState = JumpState.Boost;
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

            if (Mathf.Abs(MoveValue.y) >= ladderEnterThresholdOnY && MoveValue.y > 0)
            {
                if (sm.detection.ladderDetector.collider2Ds.Count > 0)
                {
                    var ladderCollider = sm.detection.ladderDetector.collider2Ds.Last();
                    if (ladderCollider.GetComponent<LadderInfo>() is { } ladderInfo)
                    {
                        if (ladderInfo.bottomCollider == ladderCollider)
                        {
                            sm.Switch(PlayerState.Ladder);
                            return;
                        }
                    }
                }
            }
            
            if (curState == JumpState.Boost)
            {
                timeAfterJump = 0;
                TargetRb2D.velocity = new Vector2(sm.MoveValue.x * moveSpeedOnAir, jumpSpeed);
                TargetRb2D.gravityScale = ascendingGravityScaleLight;
                sm.PlayAnim(AnimationType.JumpRise);
                curState = JumpState.Rise;
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
                        sm.PlayAnim(AnimationType.JumpMid);
                    }
                    else if (timeAfterJump >= smallJumpTime)
                    {
                        if (isReleased)
                        {
                            TargetRb2D.gravityScale = ascendingGravityScaleHeavy;
                            sm.PlayAnim(AnimationType.JumpMid);
                        }
                    }

                    if (curVelocityY <= 0)
                    {
                        curState = JumpState.Fall;
                        Debug.Log($"Reach Highest: {sm.transform.position.y}");
                        TargetRb2D.gravityScale = fallGravityScale;
                        sm.PlayAnim(AnimationType.JumpFall);
                    }
                }

                #endregion

                // Set sprite position
                if (curVelocityX != 0)
                {
                    sm.SetDirection(curVelocityX < 0 ? PlayerDirection.Back : PlayerDirection.Front);
                }
                
                #region HangDetection

                // Check Hangable
                // if (curVelocity.y <= 0 && CanHung && !sm.detection.grounded)
                if (CanHang && !sm.detection.grounded)
                {
                    sm.Switch(PlayerState.Hang);
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
                            sm.Switch(PlayerState.Move);
                        }
                        else
                        {
                            Debug.Log($"Jump use time :{timeAfterJump}");
                            sm.FixPosition();
                            sm.Switch(PlayerState.Idle);
                        }
                    }
                }
                
                #endregion
            }
        }

        public override void StateExit()
        {
            TargetRb2D.gravityScale = 0;
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