using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerControllerTest;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Ladder : IState
{
    [SerializeField] private float climbSpeed;
    [SerializeField] private float snapSpeed;
    [SerializeField] private float climbThresholdY;
    [SerializeField] private float snapThreshold;

    [SerializeField] private LadderInfo ladderInfo;
    [SerializeField] private LadderState curState;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private Vector2 snapTargetPos;
    [SerializeField] private bool inited;

    public override void StateEnter(FSM.PlayerState preState)
    {
        // Turn collider off
        sm.playerCollider.enabled = false;

        collider2D = sm.detection.ladderDetector.collider2Ds.Last();

        ladderInfo = collider2D.GetComponent<LadderInfo>();

        inited = false;

        if (collider2D == ladderInfo.topCollider)
        {
            snapTargetPos = ladderInfo.topPoint.position.xy();
            SwitchState(LadderState.SnapUp);
        }
        else
        {
            snapTargetPos = new Vector2(collider2D.transform.position.x,
                Mathf.Clamp(sm.targetRb2D.transform.position.y, ladderInfo.bottomPoint.position.y,
                    ladderInfo.climbMaxPoint.position.y));
            SwitchState(LadderState.SnapDown);
        }
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if ((curState == LadderState.Waiting || curState == LadderState.Climbing) && context.canceled == false)
        {
            sm.Switch(FSM.PlayerState.Fall);
        }
    }

    public override void StateExit()
    {
        sm.targetRb2D.velocity = Vector2.zero;
        sm.playerCollider.enabled = true;
        sm.animator.enabled = true;
    }

    private void SwitchState(LadderState targetState)
    {
        curState = targetState;
        inited = false;
    }
    
    private void onFixedUpdate()
    {
        switch (curState)
        {
            case LadderState.SnapDown:
                 sm.animator.enabled = true;
                
                var snapDownDistance = snapTargetPos - sm.character.transform.position.xy();

                if (snapDownDistance.magnitude <= snapThreshold)
                {
                    sm.character.transform.SetXY(snapTargetPos);
                    sm.targetRb2D.velocity = Vector2.zero;
                    SwitchState(LadderState.Waiting);
                }
                else
                {
                    var velocity = snapDownDistance.normalized * snapSpeed;
                    sm.targetRb2D.velocity = velocity;
                }

                break;
            case LadderState.SnapUp:
                sm.animator.enabled = true;
                var snapUpDistance = snapTargetPos - sm.character.transform.position.xy();

                if (snapUpDistance.magnitude <= snapThreshold)
                {
                    sm.character.transform.SetXY(snapTargetPos);
                    sm.targetRb2D.velocity = Vector2.zero;
                    SwitchState(LadderState.ClimbDown);
                }
                else
                {
                    var velocity = snapUpDistance.normalized * snapSpeed;
                    sm.targetRb2D.velocity = velocity;
                }

                break;
            case LadderState.ClimbDown:
                if (!inited)
                {
                    sm.character.transform.SetXY(ladderInfo.climbMaxPoint.position);
                    sm.animator.enabled = true;
                    sm.PlayAnim(FSM.AnimationType.LadderClimbFinishReverse);
                    inited = true;
                }

                if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    if (Mathf.Abs(sm.MoveValue.y) >= climbThresholdY)
                    {
                        if (sm.MoveValue.y > 0)
                        {
                            SwitchState(LadderState.ClimbUp);
                        }
                        else
                        {
                            SwitchState(LadderState.Climbing);
                            TryClimb();
                        }
                    }
                    else
                    {
                        SwitchState(LadderState.Waiting);
                    }
                }

                break;
            case LadderState.Climbing:
                sm.animator.enabled = true;
                if (transform.position.y > ladderInfo.climbMaxPoint.position.y)
                {
                    sm.targetRb2D.velocity = Vector2.zero;
                    SwitchState(LadderState.ClimbUp);
                }
                else if (transform.position.y < ladderInfo.bottomPoint.position.y)
                {
                    sm.Switch(FSM.PlayerState.Idle);
                }
                else
                {
                    if (Mathf.Abs(sm.MoveValue.y) < climbThresholdY)
                    {
                        sm.targetRb2D.velocity = Vector2.zero;
                        SwitchState(LadderState.Waiting);
                    }
                    else
                    {
                        TryClimb();
                    }
                }

                break;
            case LadderState.Waiting:
                if (!inited)
                {
                    sm.animator.enabled = false;
                    sm.targetRb2D.velocity = Vector2.zero;
                }

                if (Mathf.Abs(sm.MoveValue.y) >= climbThresholdY)
                {
                    if (sm.MoveValue.y > 0 && transform.position.y >= ladderInfo.climbMaxPoint.position.y)
                    {
                        sm.animator.enabled = true;
                        SwitchState(LadderState.ClimbUp);
                    }
                    else if (sm.MoveValue.y < 0 && transform.position.y < ladderInfo.bottomPoint.position.y)
                    {
                        sm.animator.enabled = true;
                        sm.targetRb2D.velocity = Vector2.zero;
                        sm.FixPosition();
                        sm.animator.enabled = true;
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                    else
                    {
                        sm.animator.enabled = true;
                        SwitchState(LadderState.Climbing);
                    }
                }
                break;
            case LadderState.ClimbUp:
                if (!inited)
                {
                    sm.animator.enabled = true;
                    sm.PlayAnim(FSM.AnimationType.LadderClimbFinish);
                    inited = true;
                }
                else
                {
                    var state = sm.animator.GetCurrentAnimatorStateInfo(0);
                    
                    if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        sm.character.transform.SetXY(ladderInfo.topPoint.position);
                        sm.Switch(FSM.PlayerState.Idle);
                    }
                }

                // TODO： Maybe later switch to animation event
 
                break;
        }
    }

    public override void StateFixedUpdate()
    {
        onFixedUpdate();
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

    private enum LadderState
    {
        SnapDown,
        SnapUp,
        ClimbDown,
        Climbing,
        Waiting,
        ClimbUp,
    }
}