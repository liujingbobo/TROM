using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MonsterHorizontalMove : EntityStateAction
{
    public Vector2 direction;
    public float speed = 5;
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.animator.Play("Run",0,0);
        fromEntity.animator.Update(0);
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        fromEntity.rBody2D.velocity = Vector2.zero;
        base.OnActionStop(reason);
    }

    protected override void Update()
    {
        base.Update();
        if (actionState == EntityActionState.InProgress)
        {
            fromEntity.spriteRenderer.flipX = fromEntity.rBody2D.velocity.x < 0;
        }
    }
    private void FixedUpdate()
    {
        if (actionState == EntityActionState.InProgress)
        {
            fromEntity.rBody2D.velocity = direction * speed;
        }
    }
}
