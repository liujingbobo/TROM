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
        fromEntity.rBody2D.velocity = Vector2.zero;
        if (reason == EntityActionStopReason.Completed)
        {
            fromEntity.idle.StartAction();
        }
    }

    private void Update()
    {
        if (state == EntityActionState.InProgress)
        {
            fromEntity.spriteRenderer.flipX = fromEntity.rBody2D.velocity.x < 0;
        }
    }

    public Vector2 direction;
    public float speed = 5;
    private void FixedUpdate()
    {
        if (state == EntityActionState.InProgress)
        {
            fromEntity.rBody2D.velocity = direction * speed;
        }
    }
}
