using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Check to see if the any objects are within sight of the agent.")]
[TaskCategory("MonsterControl")] 
public class SetMonsterIdle : Action
{
    public MonsterController controller;

    public override TaskStatus OnUpdate()
    {
        controller.SetIdle();
        return TaskStatus.Success;
    }
}
