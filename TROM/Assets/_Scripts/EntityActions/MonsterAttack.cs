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
        fromEntity.animatorHelper.OnAnimationEventTriggered.AddListener(OnAttack);
    }
    protected override void OnActionStop(EntityActionStopReason reason)
    {
        base.OnActionStop(reason);
        fromEntity.animatorHelper.OnAnimationEventTriggered.RemoveListener(OnAttack);
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
            worldPos = transform.position.OffsetX(fromEntity.spriteRenderer.flipX? -1f:1f).OffsetY(1f),
            localScale = 2 * Vector3.one,
            duration = 0.1f,
            damage = 1,
            team = GameTeam.Enemy,
        };
        AttackReleaseExtension.CreateAttackRelease2D(attackInfo);
    }
}
