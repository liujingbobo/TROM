using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Check to see if the any objects are within sight of the agent.")]
[TaskCategory("MonsterControl")]
[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WanderIcon.png")]
public class SetMonsterHorizontalMove : Action
{
    public MonsterController controller;

    public SharedTransform entityTransform;
    public SharedTransform memoryTransform;
    
    public override TaskStatus OnUpdate()
    {
        var xDirection = memoryTransform.Value.position - entityTransform.Value.position;
        controller.MoveTo(xDirection.SetY(0).normalized);
        return TaskStatus.Success;
    }
}
