using System;
using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using UnityEngine;

public class S_Attack : IState
{
    private bool inited;
    
    public override void StateEnter(PlayerState preState, params object[] objects)
    {
        sm.targetRb2D.gravityScale = 1f;
        inited = false;
        sm.targetRb2D.velocity = Vector2.zero;
    }

    public override void StateFixedUpdate()
    {
        if (!inited)
        {
            sm.PlayAnim(AnimationType.Attack);
            inited = true;
        }
        else
        {
            if (sm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                sm.Switch(PlayerState.Idle);
            }
        }
    }

    public override void StateExit()
    {
        
    }

    public void OnAnimationEventTriggered(AnimationEventType type, string data)
    {
        if (type == AnimationEventType.CustomStringEvent && data == "attack")
        {
            SpawnAttackHitBox();
        }
    }
    
    //TODO get playerSpriteRenderer in a better way?
    public SpriteRenderer playerSpriteRenderer;
    public void SpawnAttackHitBox()
    {
        var attackInfo = new AttackReleaseInfo()
        {
            objectName = "attackRelease",
            fromEntity = null,
            worldPos = transform.position.OffsetX(playerSpriteRenderer.flipX? -1f:1f).OffsetY(1f), //TODO find a way to better get this position?
            localScale = 2 * Vector3.one,
            duration = 0.1f,
            damage = 1,
            team = GameTeam.Player,
        };
        AttackReleaseExtension.CreateAttackRelease2D(attackInfo);
    }
}
