using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Jump : IState
    {
        private Rigidbody2D TargetRb2D => sm.rigidbody2D;
        private PlayerInput input;
        
        private float _horizontalMoveSpeed = 0;
        private bool jumped = false;
        private float preVerVal;

        public override void StateEnter()
        {
            jumped = false;
            preVerVal = 0;
        }
        
        public override void StateFixedUpdate()
        {
            if (jumped == false)
            {
                TargetRb2D.velocity = new Vector2(sm.MoveValue.x * sm.moveSpeedOnAir, sm.jumpSpeed);
                jumped = true;
            }
            else
            {
                float curVerVel = TargetRb2D.velocity.y;

                if (sm.controller.grounded && preVerVal < 0)
                {
                    sm.Switch(FSM.PlayerState.Idle);
                }
                else if (curVerVel < 0)
                {
                    TargetRb2D.velocity = new Vector2(sm.MoveValue.x * sm.moveSpeedOnAir, TargetRb2D.velocity.y);
                    TargetRb2D.gravityScale = sm.fallGravity;
                }

                preVerVal = curVerVel;
            }
        }

        public override void StateExit()
        {
            TargetRb2D.gravityScale = sm.regularGravity;
        }
    }
}