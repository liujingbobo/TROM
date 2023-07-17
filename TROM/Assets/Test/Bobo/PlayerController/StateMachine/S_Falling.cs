using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Falling : IState
{

    [ShowInInspector] private float timeAfterEnter;
    [ShowInInspector] private bool startedCounting;

    [ShowInInspector] private bool CanJump => timeAfterEnter <= sm.coyoteTime;
    
    public override void StateEnter()
    {
        sm.targetRb2D.gravityScale = sm.fallGravityScale;
        timeAfterEnter = 0;
        startedCounting = false;
    }


    public override void OnJump(InputAction.CallbackContext context)
    {
        if (CanJump)
        {
            sm.Switch(FSM.PlayerState.Jump);
        }
    }

    public override void StateFixedUpdate()
    {
        if (!startedCounting)
        {
            startedCounting = true;
        }
        else
        {
            timeAfterEnter += Time.fixedDeltaTime;
        }

        if (!sm.detection.grounded)
        {
            return;
        }

        if (sm.targetRb2D.velocity.y == 0)
        {
            sm.Switch(sm.MoveValue != Vector2.zero ? FSM.PlayerState.Move : FSM.PlayerState.Idle);
        }
    }
}
