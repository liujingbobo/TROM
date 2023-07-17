using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdle : EntityAction<GameEntity>
{
    protected override void OnActionStart()
    {
        fromEntity.animator.Play("Idle",0);
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
    }
}
