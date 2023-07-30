using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStagger : EntityStateAction
{
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.animator.Play("SwordGuardImpact",0,0);
        fromEntity.animator.Update(0);
    }
    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
    }
}
