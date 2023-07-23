using System.Collections;
using System.Collections.Generic;
using TimUtility;
using UnityEngine;

public class MonsterIdle : EntityStateAction
{
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.animator.Play("Idle",0,0);
        fromEntity.animator.Update(0);
        fromEntity.rBody2D.velocity = Vector2.zero;
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
    }
}
