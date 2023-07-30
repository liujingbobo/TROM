using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MonsterControl")]
public class TriggerMonsterAttack : Action
{
	public MonsterController monsterController;
	public SharedTransform targetTransform;
	public override void OnStart()
	{
		monsterController.AttackAt(targetTransform.Value.position);
	}

	public override TaskStatus OnUpdate()
	{
		var action = (MonsterAttack)monsterController.GetState(nameof(MonsterAttack));
		if (action.GetState() == EntityActionState.InProgress)
		{
			return TaskStatus.Running;
		}
		else
		{
			return TaskStatus.Success;
		}
	}
}