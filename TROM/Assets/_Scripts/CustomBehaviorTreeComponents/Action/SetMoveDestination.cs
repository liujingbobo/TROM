using System.Collections;
using System.Collections.Generic;
using System.IO;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


[TaskDescription("Set Path finding destination of a PlatformAILerp Class")]
[TaskCategory("MonsterControl")]
public class SetMoveDestination : Action
{
    public SharedBehaviour monsterControllerSB;
    public SharedVector3 targetPosition;

    private MonsterController mc;
    public override void OnStart()
    {
        mc = monsterControllerSB.Value as MonsterController;
        mc.MoveTo(targetPosition.Value);
    }
    
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
