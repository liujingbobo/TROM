using System.Collections;
using System.Collections.Generic;
using System.IO;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


[TaskDescription("Set Path finding destination of a PlatformAILerp Class")]
[TaskCategory("MonsterControl")]
public class PlatformMoveTo : Action
{
    public PlatformAILerp movementAI;
    public SharedTransform movementTargetTransform;

    public override void OnStart()
    {
        movementAI.SetDestination(movementTargetTransform.Value.position);
    }
    
    public override TaskStatus OnUpdate()
    {
        if (movementAI.ReachedDestination)
        {
            return TaskStatus.Success;
        }
        
        movementAI.SetDestination(movementTargetTransform.Value.position);
        
        return TaskStatus.Running;
    }
}
