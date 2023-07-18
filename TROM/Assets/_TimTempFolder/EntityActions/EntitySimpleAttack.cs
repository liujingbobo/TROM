using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySimpleAttack : EntityAction<GameEntity>
{
    protected override void OnActionStart()
    {
        fromEntity.animator.Play("Punch01",0);
        fromEntity.animatorHelper.OnAnimationEventTriggered.AddListener(OnAnimEvent);
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        fromEntity.rBody2D.velocity = Vector2.zero;
        if (reason == EntityActionStopReason.Completed)
        {
            fromEntity.idle.StartAction();
        }
        fromEntity.animatorHelper.OnAnimationEventTriggered.RemoveListener(OnAnimEvent);

    }

    private void Update()
    {
    }

    private void OnAnimEvent(AnimationEventType type, string data)
    {
        if (type == AnimationEventType.AnimationEnd)
        {
            StopAction(EntityActionStopReason.Completed);
        }
    }
}
