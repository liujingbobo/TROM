using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerControllerTest;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Hang : IState
{
    // Hang
    [SerializeField] private float hangSnappingSpeed = 5;
    [SerializeField] private float hangSnapRange = 0.5f;
    [SerializeField] private Vector2 hangStartOffset;
    [SerializeField] private Vector2 hangEndOffset;
    [SerializeField] private float climbUpThresholdY = 0.1f;

    private HangObjectInfo hangInfo;
    private Collider2D targetCollider;
    private HungState curState;
    private bool inited;
    private bool jumped;
    private Vector2 startTargetPos;
    private Vector2 endTargetPos;

    public override void StateEnter(FSM.PlayerState preState)
    {
        inited = false;
        jumped = false;

        curState = HungState.SnappingDown;

        sm.targetRb2D.gravityScale = 0;
        
        targetCollider = preState switch
        {
            FSM.PlayerState.Move => sm.detection.downHangDetector.collider2Ds.Last(),
            FSM.PlayerState.Idle => sm.detection.downHangDetector.collider2Ds.Last(),
            FSM.PlayerState.Jump => sm.detection.upperHangDetector.collider2Ds.Last(),
            FSM.PlayerState.Fall => sm.detection.upperHangDetector.collider2Ds.Last()
        };
        
        if (targetCollider.gameObject.GetComponent<HangObjectInfo>() is { } targetHangInfo)
        {
            this.hangInfo = targetHangInfo;
        }
        sm.SetDirection(hangInfo.onHangDirection);
        
        CalculatePosition();
        
        curState = preState switch
        {
            FSM.PlayerState.Move => HungState.SnappingUp,
            FSM.PlayerState.Idle => HungState.SnappingUp,
            FSM.PlayerState.Jump => HungState.SnappingDown,
            FSM.PlayerState.Fall => HungState.SnappingDown
        };

        sm.playerCollider.enabled = false;
    }

    private void CalculatePosition()
    {
        var colPos = targetCollider.transform.position.xy();
        var offset = new Vector2((hangInfo.onHangDirection == FSM.PlayerDirection.Front ? 1 : -1) * hangStartOffset.x, hangStartOffset.y);
        var endoffset = new Vector2((hangInfo.onHangDirection == FSM.PlayerDirection.Front ? 1 : -1) * hangEndOffset.x, hangEndOffset.y);

        endTargetPos = colPos + endoffset;
        startTargetPos = colPos + offset;
    }

    public override void StateFixedUpdate()
    {
        switch (curState)
        {
            // Front top to bot
            case HungState.SnappingUp:
                var snappingUpDistance = endTargetPos - sm.character.transform.position.xy();

                if (snappingUpDistance.magnitude <= hangSnapRange)
                {
                    sm.character.transform.position = endTargetPos;
                    sm.targetRb2D.velocity = Vector2.zero;
                    
                    curState = HungState.ClimbDown;
                    inited = false;
                }
                else
                {
                    var snapSpeed  = snappingUpDistance.normalized * hangSnappingSpeed;
                    sm.targetRb2D.velocity = snapSpeed;
                }
                break;
            case HungState.ClimbDown:
                if (!inited)
                {
                    sm.character.transform.position = startTargetPos;
                    inited = true;
                    sm.PlayAnim(FSM.AnimationType.LedgeClimbPreviewReverse);
                }
                else
                {
                    if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        sm.character.transform.position = startTargetPos;
                        sm.PlayAnim(FSM.AnimationType.LedgeHangPreview);
                        curState = HungState.Waiting;
                    }
                }
                break;
            // Front bot to top
            case HungState.SnappingDown:
                if (!inited)
                {
                    inited = true;
                }
                else
                {
                    var distance = startTargetPos - sm.character.transform.position.xy();

                    if (distance.magnitude <= hangSnapRange)
                    {
                        sm.character.transform.position = startTargetPos;
                        sm.targetRb2D.velocity = Vector2.zero;
                        curState = HungState.Waiting;
                        sm.PlayAnim(FSM.AnimationType.LedgeHangPreview);
                    }
                    else
                    {
                        distance = distance.normalized * hangSnappingSpeed;
                        sm.targetRb2D.velocity = distance;
                    }
                }
                break;
            case HungState.Waiting:
                if (sm.MoveValue.y >= climbUpThresholdY && jumped)
                {
                    sm.PlayAnim(FSM.AnimationType.LedgeClimbPreview);
                    curState = HungState.ClimbUp;
                }
                else if(jumped)
                {
                    sm.Switch(FSM.PlayerState.Fall);
                }
                break;
            case HungState.ClimbUp:
                if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    sm.character.transform.position = endTargetPos;
                    // sm.FixPosition();
                    if (sm.MoveValue.x != 0)
                    {
                        sm.Switch(FSM.PlayerState.Move);
                    }
                    else
                    {
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                }
                break;
        }
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if (curState == HungState.Waiting && jumped == false && context.canceled == false)
        {
            jumped = true;
        }
    }
    
    public override void StateExit()
    {
        sm.playerCollider.enabled = true;
    }

    private enum HungState
    {
        ClimbDown,
        SnappingUp,
        SnappingDown,
        Waiting,
        ClimbUp
    }
}