using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Idle : IState
    {
        [BoxGroup] public float horizontalMoveThreshold = 0.1f;
        
        public override void StateEnter(FSM.PlayerState preState)
        {
            sm.targetRb2D.gravityScale = 0;
            sm.targetRb2D.velocity = Vector2.zero;
            sm.PlayAnim(FSM.AnimationType.Idle);
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if (Mathf.Abs(sm.MoveValue.x) > horizontalMoveThreshold)
            {
                sm.Switch(FSM.PlayerState.Move);
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
            if (!sm.detection.grounded)
            {
                sm.Switch(FSM.PlayerState.Falling);
            }
        }
    }
}
