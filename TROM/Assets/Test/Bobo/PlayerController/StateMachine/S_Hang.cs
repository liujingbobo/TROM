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
    public float hangSnappingSpeed = 5;
    public float hangSnapRange = 0.5f;
    public Vector2 hangStartOffset;
    public Vector2 hangEndOffset;
    
    private Collider2D targetCollider;
    private Collider2D playerCollider;
    private Vector2 enterInput;
    private HungState curState;
    private bool inited;
    private bool jumped;
    private Vector2 startTargetPos;
    private Vector2 endTargetPos;
    
    public override void StateEnter(FSM.PlayerState preState)
    {
        inited = false;
        jumped = false;
        curState = HungState.Snapping;
        enterInput = sm.MoveValue;
        sm.targetRb2D.gravityScale = 0;
        targetCollider = sm.detection.hangDetector.collider2Ds.Last();
        sm.animator.Play("LedgeHangPreview");
        enterInput = sm.MoveValue;
        var colPos = targetCollider.transform.position.xy();
        var offset = new Vector2((sm.MoveValue.x > 0 ? 1 : -1) * hangStartOffset.x, hangStartOffset.y);
        var endoffset = new Vector2((sm.MoveValue.x > 0 ? 1 : -1) * hangEndOffset.x,hangEndOffset.y);
        endTargetPos = colPos + endoffset;
        startTargetPos = colPos + offset;
        playerCollider = sm.targetRb2D.transform.GetComponent<Collider2D>();
        playerCollider.enabled = false;
    }
    
    public override void StateFixedUpdate()
    {
        switch (curState)
        {
            case HungState.Snapping:
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
                    }
                    else
                    {
                        distance = distance.normalized * hangSnappingSpeed;
                        sm.targetRb2D.velocity = distance;
                    }
                }
                break;
            case HungState.Waiting:
                if (jumped)
                {
                    sm.animator.Play("LedgeClimbPreview");
                    curState = HungState.Animating;
                }
                break;
            case HungState.Animating:
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
    public override void OnMove(InputAction.CallbackContext context)
    {
    }

    public override void StateExit()
    {
        playerCollider.enabled = true;
    }

    private enum HungState
    {
        Snapping,
        Waiting,
        Animating
    }
}
