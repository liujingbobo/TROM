using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Falling : IState
{
    // Falling
    [BoxGroup("Falling")]
    public float fallingGroundCheckGap = 0.1f;
    [BoxGroup("Jump")] public float airAcceleration; 
    [BoxGroup("Jump")] public float moveSpeedOnAir; // Separate the speed on air and ground
    [BoxGroup("Jump")] public float horizontalMoveThreshold; 
    [BoxGroup("Jump")] public float fallGravityScale = 1.5f;
    [BoxGroup("Jump")] public float coyoteTime;
    
    private Rigidbody2D TargetRb2D => sm.targetRb2D;
    private Vector2 MoveValue => sm.MoveValue;

    private float timeAfterEnter;
    private bool started;
    private bool CanJump => timeAfterEnter <= coyoteTime;
    private bool CanCheckGround => timeAfterEnter >= fallingGroundCheckGap;
    
    public override void StateEnter(PlayerState preState, params object[] objects)
    {
        sm.targetRb2D.bodyType = RigidbodyType2D.Dynamic;
        sm.targetRb2D.gravityScale = fallGravityScale;
        timeAfterEnter = 0;
        started = false;
        sm.PlayAnim(AnimationType.JumpFall);
    }


    public override void OnJump(InputAction.CallbackContext context)
    {
        if (context is { started: true, canceled: false } && CanJump)
        {
            sm.Switch(PlayerState.Jump);
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
        
        var speedChangeValue = MoveValue.x * airAcceleration * Time.fixedDeltaTime;
        var curVelocity = TargetRb2D.velocity;
        var curY = curVelocity.y;
        var curX = curVelocity.x;
        if (MoveValue != Vector2.zero)
        {
            // Move
            curX += speedChangeValue;
            curX = Mathf.Clamp(curX, -moveSpeedOnAir,
                moveSpeedOnAir);
        }
        else
        {
            // Stop or deceleration\
            if (curX != 0)
            {
                if (curX > 0)
                {
                    curX -= airAcceleration * Time.fixedDeltaTime;
                    curX = Mathf.Max(curX, 0);
                }
                else
                {                            
                    curX += airAcceleration * Time.fixedDeltaTime;
                    curX = Mathf.Min(curX, 0);
                }
            }
        }
        
        if (curX != 0)
        {
            sm.SetDirection(curX < 0 ? PlayerDirection.Back : PlayerDirection.Front);
        }

        TargetRb2D.velocity = new Vector2(curX, curY);

        if (!CanCheckGround || !sm.detection.isGrounded)
        {
            return;
        }

        if (sm.detection.isGrounded)
        {
            sm.Switch(sm.MoveValue.x != 0 ? PlayerState.Move : PlayerState.Idle);
        }
    }
}
