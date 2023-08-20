using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using Spine.Unity;
using UnityEngine;

public class MonsterMove : EntityStateAction
{
    public AILerp ai;
    public Vector2 destination;
    public float speed = 5;
    public string animationName;
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.SkeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        ai.canMove = true;
        ai.canSearch = true;
    }
    public void SetDestination(Vector3 pos)
    {
        destination = pos;
        ai.destination = destination;
    }
    protected override void OnActionStop(EntityActionStopReason reason)
    {
        ai.canMove = false;
        ai.canSearch = false;
        base.OnActionStop(reason);
    }

    private Vector3 _lastPosition;
    protected override void Update()
    {
        base.Update();
        if (actionState == EntityActionState.InProgress)
        {
            var curPos = transform.position;
            var diffVector = curPos - _lastPosition;
            _lastPosition = curPos;
            if (diffVector.magnitude > 0.001f)
            {
                var shouldFaceRight = diffVector.x > 0;
                fromEntity.SetFacingDirection(shouldFaceRight ? FacingDirection.Right : FacingDirection.Left);
            }
            if (ai.reachedDestination)
            {
                StopAction(EntityActionStopReason.Completed);
            }
        }
    }
}
