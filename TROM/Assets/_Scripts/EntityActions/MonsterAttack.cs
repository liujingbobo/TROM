using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TimUtility;
using UnityEngine;

public class MonsterAttack : EntityStateAction
{
    public Vector2 targetPoint;
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.animator.Play("Punch01",0,0);
        fromEntity.animator.Update(0);
    }
    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
    }
}
