using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MonsterStagger : EntityStateAction
{
    public string animationName;
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.SkeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
    }
    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
    }
}
