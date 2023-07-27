using System.Collections;
using System.Collections.Generic;
using System.IO;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


[TaskDescription("Check to see if the any objects are within sight of the agent.")]
[TaskCategory("MonsterControl")]
public class PlatformMoveTo : Action
{
    public PlatformAStarMovement movement;
    public SharedTransform targetTransform;

    public override void OnStart()
    {
        movement.targetTransform = targetTransform.Value;
        movement.ChaseToTargetTransform();
    }
    
    public override TaskStatus OnUpdate()
    {
        if (movement.reachedEndOfPath)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
