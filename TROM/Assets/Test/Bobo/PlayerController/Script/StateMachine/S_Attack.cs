using System;
using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using UnityEngine;

public class S_Attack : IState
{
    private bool inited;
    
    public override void StateEnter(PlayerState preState)
    {
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
                var position = sm.transform.position;
                position = position.SetX(position.x + 0.5f);
                sm.transform.position = position;
                sm.Switch(PlayerState.Idle);
            }
        }
    }

    public override void StateExit()
    {
        
    }
    
}
