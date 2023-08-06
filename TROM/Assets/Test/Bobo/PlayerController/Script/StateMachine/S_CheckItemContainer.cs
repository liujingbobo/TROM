using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using UnityEngine;

public class S_CheckItemContainer : IState
{
    public override void StateEnter(PlayerState preState, params object[] objects)
    {
        base.StateEnter(preState);

        // Pass the reference of container. 
        if (objects[0] is ItemContainer container)
        {
            sm.PlayAnim(AnimationType.CheckItemContainer);
            BackpackUI.Singleton.OpenUI(container, () =>
            {
                sm.interactor2D.StopInteract();
                Switch(PlayerState.Idle);
            });
        }
        else
        {
            Switch(PlayerState.Idle);
        }
    }
}
