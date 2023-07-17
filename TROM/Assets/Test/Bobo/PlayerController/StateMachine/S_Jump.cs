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
            Debug.Log("Enter First");
            jumped = false;
            preVerVal = 0;
        }
        
        public override void StateFixedUpdate()
        {
            Debug.Log("Jump Fixed Enter");
            Debug.Log($"Fixed Updates: {TargetRb2D.transform.position}");
            Debug.Log($"Fixed Updates: Grounded: {sm.controller.grounded}");
            
            if (jumped == false)
            {
                Debug.Log("Jump Fixed Update no jump");
                TargetRb2D.velocity = new Vector2(sm.MoveValue.x * sm.moveSpeedOnAir, sm.jumpSpeed);
                jumped = true;
            }
            else
            {
                Debug.Log("Jump Fixed Update jumped");
                float curVerVel = TargetRb2D.velocity.y;

                if (curVerVel <= 0 && sm.controller.grounded)
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

        public override void StateUpdate()
        {
            Debug.Log("Jump Update Enter");
            Debug.Log($"Updates: {TargetRb2D.transform.position}");
            Debug.Log($"Updates: Grounded: {sm.controller.grounded}");
        }

        public override void StateExit()
        {
            TargetRb2D.gravityScale = sm.regularGravity;
        }
    }
}