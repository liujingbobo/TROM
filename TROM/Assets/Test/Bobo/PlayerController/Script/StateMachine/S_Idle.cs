using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Idle : IState
    {

        [SerializeField] private float hangYThreshold = 0.1f;
        [BoxGroup] public float horizontalMoveThreshold = 0.1f;
        [BoxGroup] public float verticalLadderThreshold = 0.1f;
        public override void StateEnter(FSM.PlayerState preState)
        {
            sm.FixPosition();
            sm.targetRb2D.gravityScale = 0;
            sm.targetRb2D.velocity = Vector2.zero;
            sm.PlayAnim(FSM.AnimationType.Idle);
        }

        public override void OnMove(InputAction.CallbackContext context)
        {
            if (Mathf.Abs(sm.MoveValue.x) > horizontalMoveThreshold)
            {
                sm.Switch(FSM.PlayerState.Move);
            }else if (Mathf.Abs(sm.MoveValue.y) > verticalLadderThreshold)
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
                                sm.Switch(FSM.PlayerState.Ladder);
                            }
                        }
                        else
                        {
                            if (sm.MoveValue.y < 0)
                            {
                                sm.Switch(FSM.PlayerState.Ladder);
                            }
                        }
                    }
                }
            }
        }

        public override void OnJump(InputAction.CallbackContext context)
        {
            if (sm.MoveValue.y <= -hangYThreshold && context is { started: true, canceled: false })
            {
                // TODO: overlap detect
                if (sm.detection.downHangDetector.collider2Ds.Count > 0)
                {
                    sm.Switch(FSM.PlayerState.Hang);
                }
            }else if (context is { started: true, canceled: false })
            {
                sm.Switch(FSM.PlayerState.Jump);
            }
        }

        public override void StateFixedUpdate()
        {
            if (!sm.detection.grounded)
            {
                sm.Switch(FSM.PlayerState.Fall);
                return;
            }

            if (Mathf.Abs(sm.MoveValue.x) > horizontalMoveThreshold)
            {
                sm.Switch(FSM.PlayerState.Move);
            }
        }
    }
}
