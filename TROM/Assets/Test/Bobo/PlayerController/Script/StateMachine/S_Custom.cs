using System;
using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using UnityEngine;
using UnityEngine.Serialization;

// Just some simple custom state, which basically play a short or simple loop animation.
public class S_Custom : IState
{
    // So it really should be a simple state, play and go. 
    public struct CustomStateInfo
    {
        public AnimationType TargetAnimationType;
        public bool IsLoop;
        public int LoopTime;
        public bool CanBeInterrupted; // So far only get hit
        public float GravityScale;
        public Action OnHit;
        public Action OnEnd;
        public Action<Action> SendCallBack; // Give call back function. 
    }

    public override void StateEnter(PlayerState preState, params object[] objects)
    {
        if (objects[0] is CustomStateInfo { } info)
        {
            
        }
        else
        {
            Switch(preState);
        }
    }

    private void FixedUpdate()
    {
        
    }

    public override void StateExit()
    {
        
    }

    public enum StateEndCondition
    {
        // For Loop Animation
        LoopTimes,
        TimePassed,
        
        // For Single Animation
        WaitTillAnimationFinished,
        
        WaitForCallBack
    }
}
