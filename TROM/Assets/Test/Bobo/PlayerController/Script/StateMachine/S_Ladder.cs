using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerControllerTest;
using UnityEngine;

public class S_Ladder : IState
{
    [SerializeField] private float climbSpeed;
    [SerializeField] private float snapSpeed;
    [SerializeField] private float climbThresholdY;
    [SerializeField] private float snapThreshold;

    private LadderInfo ladderInfo;
    private bool inited;
    private LadderState curState;
    private Collider2D collider2D;
    private Vector2 snapTargetPos;

    public override void StateEnter(FSM.PlayerState preState)
    {
        inited = false;
        sm.playerCollider.enabled = false;

        collider2D = sm.detection.ladderDetector.collider2Ds.Last();
        ladderInfo = collider2D.GetComponent<LadderInfo>();

        if (collider2D == ladderInfo.topCollider)
        {
            curState = LadderState.SnapUp;
            snapTargetPos = ladderInfo.topPoint.position.xy();
        }
        else
        {
            curState = LadderState.SnapDown;
        }
    }

    public override void StateExit()
    {
        sm.playerCollider.enabled = true;
    }

    public override void StateFixedUpdate()
    {
        switch (curState)
        {
            case LadderState.SnapDown:
                if (!inited)
                {
                    inited = true;
                }
                else
                {
                    var distance = snapTargetPos - sm.character.transform.position.xy();

                    if (distance.magnitude <= snapThreshold)
                    {
                        sm.character.transform.position = snapTargetPos;
                        sm.targetRb2D.velocity = Vector2.zero;
                        curState = LadderState.Waiting;
                    }
                    else
                    {
                        var velocity = distance.normalized * snapSpeed;
                        sm.targetRb2D.velocity = velocity;
                    }
                }
                break;
            case LadderState.SnapUp:
                if (!inited)
                {
                    inited = true;
                }
                else
                {
                    var distance = snapTargetPos - sm.character.transform.position.xy();

                    if (distance.magnitude <= snapThreshold)
                    {
                        sm.character.transform.position = snapTargetPos;
                        sm.targetRb2D.velocity = Vector2.zero;
                        sm.PlayAnim(FSM.AnimationType.LadderClimbFinishReverse);
                        curState = LadderState.ClimbDown;
                    }
                    else
                    {
                        var velocity = distance.normalized * snapSpeed;
                        sm.targetRb2D.velocity = velocity;
                    }
                }
                break;
            case LadderState.ClimbDown:
                if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    sm.character.transform.SetXY(ladderInfo.climbMaxPoint.position);
                    // sm.FixPosition();
                    if (Mathf.Abs(sm.MoveValue.y) >= climbThresholdY)
                    {
                        if (sm.MoveValue.y > 0)
                        {
                            sm.PlayAnim(FSM.AnimationType.LadderClimbFinish);
                            curState = LadderState.ClimbUp;
                        }else
                        {
                            curState = LadderState.Climbing;
                            TryClimb();
                        }
                    }
                    else
                    {
                        curState = LadderState.Waiting;
                    }
                }
                break;
            case LadderState.Climbing:
                if (transform.position.y >= ladderInfo.climbMaxPoint.position.y)
                {
                    curState = LadderState.ClimbUp;
                    sm.targetRb2D.velocity = Vector2.zero;
                }else if (transform.position.y <= ladderInfo.bottomPoint.position.y)
                {
                    sm.targetRb2D.velocity = Vector2.zero;
                    sm.FixPosition();
                    sm.Switch(FSM.PlayerState.Idle);
                    return;
                }
                else
                {
                    if (Mathf.Abs(sm.MoveValue.y) < climbThresholdY)
                    {
                        sm.targetRb2D.velocity = Vector2.zero;
                        curState = LadderState.Waiting;
                    }
                    else
                    {
                        TryClimb();
                    }
                }
                break;
            case LadderState.Waiting:
                if (Mathf.Abs(sm.MoveValue.y) >= climbThresholdY)
                {
                    TryClimb();
                    curState = LadderState.Climbing;
                }
                else
                {
                    sm.targetRb2D.velocity = Vector2.zero;
                }
                break;
            case LadderState.ClimbUp:
                if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    sm.character.transform.SetXY(ladderInfo.climbMaxPoint.position);
                    // sm.FixPosition();
                    if (Mathf.Abs(sm.MoveValue.y) >= climbThresholdY)
                    {
                        if (sm.MoveValue.y > 0)
                        {
                            sm.PlayAnim(FSM.AnimationType.LadderClimbFinish);
                            curState = LadderState.ClimbUp;
                        }else
                        {
                            curState = LadderState.Climbing;
                            TryClimb();
                        }
                    }
                    else
                    {
                        curState = LadderState.Waiting;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void TryClimb()
    {
        if (sm.MoveValue.y > 0)
        {
            sm.PlayAnim(FSM.AnimationType.LadderClimb);
            sm.targetRb2D.velocity = new Vector2(0, climbSpeed);
        }
        else if (sm.MoveValue.y < 0)
        {
            sm.PlayAnim(FSM.AnimationType.LadderClimbReverse);
            sm.targetRb2D.velocity = new Vector2(0, -climbSpeed);
        }
    }

    public enum LadderState
    {
        SnapDown,
        SnapUp,
        ClimbDown,
        Climbing,
        Waiting,
        ClimbUp
    }
}