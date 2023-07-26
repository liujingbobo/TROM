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
                break;
            case LadderState.ClimbDown:
                break;
            case LadderState.Climbing:
                break;
            case LadderState.Waiting:
                break;
            case LadderState.ClimbUp:
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
