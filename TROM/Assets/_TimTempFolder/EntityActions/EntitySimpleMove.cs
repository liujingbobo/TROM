using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EntitySimpleMove : EntityAction<GameEntity>
{
    public Vector2 direction;
    public float speed = 5;
    protected override void OnActionStart()
    {
        fromEntity.animator.Play("Run",0);
        fromEntity.animator.Update(0);
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        fromEntity.rBody2D.velocity = Vector2.zero;
        if (reason == EntityActionStopReason.Completed)
        {
            fromEntity.idleAction.StartAction();
        }
    }

    private void Update()
    {
        if (state == EntityActionState.InProgress)
        {
            fromEntity.spriteRenderer.flipX = fromEntity.rBody2D.velocity.x < 0;
        }
    }
    private void FixedUpdate()
    {
        if (state == EntityActionState.InProgress)
        {
            fromEntity.rBody2D.velocity = direction * speed;
        }
    }
}
