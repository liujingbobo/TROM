using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using TimUtility;
using UnityEngine;

public class MonsterAttack : EntityStateAction
{
    public string attackAnimationName;
    public Vector2 targetPoint;
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.SkeletonAnimation.AnimationState.SetAnimation(0, attackAnimationName, false);
        //fromEntity.animatorHelper.OnAnimationEventTriggered.AddListener(OnAttack);
    }
    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
        //fromEntity.animatorHelper.OnAnimationEventTriggered.RemoveListener(OnAttack);
    }
    private void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // Check event data name
        if (e.Data.Name == "yourEventName")
        {
            // Do something in response to the event.
            Debug.Log("Spine event triggered!");
            SpawnAttackHitBox();
        }
    }
    
    public void OnAttack(AnimationEventType type, string data)
    {
        if (type == AnimationEventType.CustomStringEvent && data == "attack")
        {
            SpawnAttackHitBox();
        }
    }
    
    public void SpawnAttackHitBox()
    {
        var attackInfo = new AttackReleaseInfo()
        {
            objectName = "attackRelease",
            fromEntity = fromEntity,
            worldPos = transform.position.OffsetX(fromEntity.IsFacingRight? 1f:-1f).OffsetY(1f),
            localScale = 2 * Vector3.one,
            duration = 0.1f,
            damage = 1,
            team = GameTeam.Enemy,
        };
        AttackReleaseExtension.CreateAttackRelease2D(attackInfo);
    }
}
