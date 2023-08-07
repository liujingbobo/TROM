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

    private HangObjectInfo _hangInfo;
    private Collider2D _targetCollider;
    private HangState _curState;
    private bool _inited;
    private bool _jumped;
    private Vector2 _startTargetPos;
    private Vector2 _endTargetPos;

    public override void StateEnter(PlayerState preState, params object[] objects)
    {
        _inited = false;
        _jumped = false;

        _curState = HangState.SnappingDown;

        sm.targetRb2D.bodyType = RigidbodyType2D.Kinematic;

        _targetCollider = preState switch
        {
            PlayerState.Move => sm.detection.downHangDetector.collider2Ds.Last(),
            PlayerState.Idle => sm.detection.downHangDetector.collider2Ds.Last(),
            PlayerState.Jump => sm.detection.upperHangDetector.collider2Ds.Last(),
            PlayerState.Fall => sm.detection.upperHangDetector.collider2Ds.Last()
        };

        if (_targetCollider.gameObject.GetComponent<HangObjectInfo>() is { } targetHangInfo)
        {
            this._hangInfo = targetHangInfo;
        }

        sm.SetDirection(_hangInfo.onHangDirection);

        CalculatePosition();

        _curState = preState switch
        {
            PlayerState.Move => HangState.SnappingUp,
            PlayerState.Idle => HangState.SnappingUp,
            PlayerState.Jump => HangState.SnappingDown,
            PlayerState.Fall => HangState.SnappingDown
        };

        sm.playerCollider.enabled = false;
    }

    private void CalculatePosition()
    {
        var colPos = _targetCollider.transform.position.xy();
        var offset = new Vector2((_hangInfo.onHangDirection == PlayerDirection.Right ? 1 : -1) * hangStartOffset.x,
            hangStartOffset.y);
        var endoffset = new Vector2((_hangInfo.onHangDirection == PlayerDirection.Right ? 1 : -1) * hangEndOffset.x,
            hangEndOffset.y);

        _endTargetPos = colPos + endoffset;
        _startTargetPos = colPos + offset;
    }

    public override void StateFixedUpdate()
    {
        switch (_curState)
        {
            // Front top to bot
            case HangState.SnappingUp:
                var snappingUpDistance = _endTargetPos - sm.character.transform.position.xy();

                if (snappingUpDistance.magnitude <= hangSnapRange)
                {
                    sm.character.transform.position = _endTargetPos;
                    sm.targetRb2D.velocity = Vector2.zero;

                    _curState = HangState.ClimbDown;
                    _inited = false;
                }
                else
                {
                    var snapSpeed = snappingUpDistance.normalized * hangSnappingSpeed;
                    sm.targetRb2D.velocity = snapSpeed;
                }

                break;
            case HangState.ClimbDown:
                if (!_inited)
                {
                    sm.character.transform.position = _startTargetPos;
                    _inited = true;
                    sm.PlayAnim(AnimationType.LedgeClimbPreviewReverse);
                }
                else
                {
                    if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    {
                        sm.character.transform.position = _startTargetPos;
                        sm.PlayAnim(AnimationType.LedgeHangPreview);
                        _curState = HangState.Waiting;
                    }
                }

                break;
            // Front bot to top
            case HangState.SnappingDown:
                if (!_inited)
                {
                    _inited = true;
                }
                else
                {
                    var distance = _startTargetPos - sm.character.transform.position.xy();

                    if (distance.magnitude <= hangSnapRange)
                    {
                        sm.character.transform.position = _startTargetPos;
                        sm.targetRb2D.velocity = Vector2.zero;
                        _curState = HangState.Waiting;
                        sm.PlayAnim(AnimationType.LedgeHangPreview);
                    }
                    else
                    {
                        distance = distance.normalized * hangSnappingSpeed;
                        sm.targetRb2D.velocity = distance;
                    }
                }

                break;
            case HangState.Waiting:
                if (sm.MoveValue.y >= climbUpThresholdY && _jumped)
                {
                    sm.PlayAnim(AnimationType.LedgeClimbPreview);
                    _curState = HangState.ClimbUp;
                }
                else if (_jumped)
                {
                    sm.Switch(PlayerState.Fall);
                }

                break;
            case HangState.ClimbUp:
                if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    sm.character.transform.position = _endTargetPos;
                    if (sm.MoveValue.x != 0)
                    {
                        sm.Switch(PlayerState.Move);
                    }
                    else
                    {
                        sm.Switch(PlayerState.Idle);
                    }
                }

                break;
        }
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if (_curState == HangState.Waiting && _jumped == false && context.canceled == false)
        {
            _jumped = true;
        }
    }

    public override void StateExit()
    {
        sm.playerCollider.enabled = true;
        sm.targetRb2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private enum HangState
    {
        ClimbDown,
        SnappingUp,
        SnappingDown,
        Waiting,
        ClimbUp
    }
}