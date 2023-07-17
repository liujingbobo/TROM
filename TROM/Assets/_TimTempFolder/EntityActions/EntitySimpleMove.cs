using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EntitySimpleMove : EntityAction<GameEntity>
{
    private Tween _delayTween;
    protected override void OnActionStart()
    {
        fromEntity.animator.Play("Run",0);
        _delayTween = DOVirtual.DelayedCall(1f, () =>
        {
            StopAction(EntityActionStopReason.Completed);
        });
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        _delayTween.Kill();
        if (reason == EntityActionStopReason.Completed)
        {
            fromEntity.idle.StartAction();
        }
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (state == EntityActionState.InProgress)
        {
            fromEntity.rBody2D.AddForce(Vector2.right * fromEntity.rBody2D.mass);
        }
    }
}
