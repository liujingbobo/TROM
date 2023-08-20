using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TimUtility;
using UnityEngine;

public class MonsterIdle : EntityStateAction
{
    public string animationName;
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.SkeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
        fromEntity.rBody2D.velocity = Vector2.zero;
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
    }
}
