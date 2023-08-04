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

        public float normalGravity = 1;
        
        public override void StateEnter(PlayerState preState)
        {
            sm.targetRb2D.gravityScale = normalGravity;
            sm.targetRb2D.velocity = Vector2.zero;
            sm.PlayAnim(AnimationType.Idle);
        }

        public override void OnAttack(InputAction.CallbackContext context)
        {
            if (context is { started: true, canceled: false })
            {
                sm.Switch(PlayerState.Attack);
            }
        }
        public override void OnMove(InputAction.CallbackContext context)
        {
            if (Mathf.Abs(sm.MoveValue.x) > horizontalMoveThreshold)
            {
                sm.Switch(PlayerState.Move);
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
                                sm.Switch(PlayerState.Ladder);
                            }
                        }
                        else
                        {
                            if (sm.MoveValue.y < 0)
                            {
                                sm.Switch(PlayerState.Ladder);
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
                    sm.Switch(PlayerState.Hang);
                }
            }else if (context is { started: true, canceled: false })
            {
                sm.Switch(PlayerState.Jump);
            }
        }

        public override void StateFixedUpdate()
        {
            if (!sm.detection.isGrounded)
            {
                sm.Switch(PlayerState.Fall);
                return;
            }

            if (Mathf.Abs(sm.MoveValue.x) > horizontalMoveThreshold)
            {
                sm.Switch(PlayerState.Move);
            }
        }
    }
}
