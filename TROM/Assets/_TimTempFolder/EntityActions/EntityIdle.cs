using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdle : EntityAction<GameEntity>
{
    protected override void OnActionStart()
    {
        fromEntity.animator.Play("Idle",0);
        fromEntity.animator.Update(0);
        fromEntity.rBody2D.velocity = Vector2.zero;
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
    }
}
