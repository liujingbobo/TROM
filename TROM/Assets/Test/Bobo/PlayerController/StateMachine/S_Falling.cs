using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Falling : IState
{
    private Rigidbody2D TargetRb2D => sm.targetRb2D;
    private Vector2 MoveValue => sm.MoveValue;

    [ShowInInspector] private float timeAfterEnter;
    [ShowInInspector] private bool started;
    [ShowInInspector] private bool CanJump => timeAfterEnter <= sm.coyoteTime;
    private bool CanCheckGround => timeAfterEnter >= sm.fallingGroundCheckGap;
    
    public override void StateEnter()
    {
        sm.targetRb2D.gravityScale = sm.fallGravityScale;
        timeAfterEnter = 0;
        started = false;
        sm.PlayAnim(FSM.AnimationType.JumpFall);
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
        if (!started)
        {
            started = true;
        }
        else
        {
            timeAfterEnter += Time.fixedDeltaTime;
        }
        
        var speedChangeValue = MoveValue.x * sm.airAcceleration * Time.fixedDeltaTime;
        var curVelocity = TargetRb2D.velocity;
        var curY = curVelocity.y;
        var curX = curVelocity.x;
        if (MoveValue != Vector2.zero)
        {
            // Move
            curX += speedChangeValue;
            curX = Mathf.Clamp(curX, -sm.moveSpeedOnAir,
                sm.moveSpeedOnAir);
        }
        else
        {
            // Stop or deceleration\
            if (curX != 0)
            {
                if (curX > 0)
                {
                    curX -= sm.airAcceleration * Time.fixedDeltaTime;
                    curX = Mathf.Max(curX, 0);
                }
                else
                {                            
                    curX += sm.airAcceleration * Time.fixedDeltaTime;
                    curX = Mathf.Min(curX, 0);
                }
            }
        }
        
        if (curX != 0)
        {
            sm.spriteRenderer.flipX = curX < 0;
        }

        TargetRb2D.velocity = new Vector2(curX, curY);

        if (!CanCheckGround || !sm.detection.grounded)
        {
            return;
        }

        if (curY != 0) return;
        
        sm.FixPosition();
        sm.Switch(sm.MoveValue.x != 0 ? FSM.PlayerState.Move : FSM.PlayerState.Idle);
    }
}
