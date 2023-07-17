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
        private PlayerInput input;
        private bool jumped;
        private float timeAfterJump;
        private bool CanCheckGround => timeAfterJump >= sm.groundCheckGap;
        public override void StateEnter()
        {
            jumped = false;
        }
        public override void StateFixedUpdate()
        {
            if (jumped == false)
            {
                timeAfterJump = 0;
                TargetRb2D.velocity = new Vector2(sm.MoveValue.x * sm.moveSpeedOnAir, sm.jumpSpeed);
                TargetRb2D.gravityScale = sm.initJumpGravityCurve.Evaluate(timeAfterJump);
                jumped = true;
            }
            else 
            {
                timeAfterJump += Time.fixedDeltaTime;

                var curVelocity = TargetRb2D.velocity;
                var curVerVel = curVelocity.y;
                
                curVelocity = new Vector2(sm.MoveValue.x * sm.moveSpeedOnAir, curVelocity.y);
                TargetRb2D.velocity = curVelocity;

                if (curVerVel < 0)
                {
                    TargetRb2D.gravityScale = sm.fallGravityScale;
                }
                else
                {
                    TargetRb2D.gravityScale = sm.initJumpGravityCurve.Evaluate(timeAfterJump);
                }
                
                if (CanCheckGround && sm.detection.grounded && curVelocity.y == 0)
                {
                    if (sm.MoveValue != Vector2.zero)
                    {
                        sm.Switch(FSM.PlayerState.Move);
                    }
                    else
                    {
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                }
            }
        }
        public override void StateExit()
        {
            TargetRb2D.gravityScale = sm.regularGravityScale;
        }
    }
}