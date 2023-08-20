using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterJump : EntityStateAction
{
    public float jumpForceMagnitude;
    public float jumpDownwardMagnitude;
    public float timeToReachMaxHeight;

    private float jumpStartTime;
    
    protected override void OnActionStart()
    {
        base.OnActionStart();
        fromEntity.rBody2D.AddForce(Vector2.up * jumpForceMagnitude, ForceMode2D.Impulse);
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        fromEntity.rBody2D.velocity = Vector2.zero;
        base.OnActionStop(reason);
    }

    private void FixedUpdate()
    {
    }
}
