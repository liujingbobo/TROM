using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Check to see if the any objects are within sight of the agent.")]
[TaskCategory("MonsterControl")]
[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WanderIcon.png")]
public class SetMonsterHorizontalMove : Action
{
    public Transform entityTransform;
    public MonsterController controller;
    
    public Vector2 direction;
    
    public override TaskStatus OnUpdate()
    {
        controller.MoveTo(direction.normalized);
        return TaskStatus.Success;
    }
}
