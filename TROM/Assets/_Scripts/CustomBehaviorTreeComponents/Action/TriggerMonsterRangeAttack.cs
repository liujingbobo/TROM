using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MonsterControl")]
public class TriggerMonsterRangeAttack : Action
{
	public MonsterController monsterController;
	public SharedTransform targetTransform;
	public override void OnStart()
	{
		monsterController.RangeAttackAt(targetTransform.Value.position);
	}

	public override TaskStatus OnUpdate()
	{
		var action = (MonsterRangedAttack)monsterController.GetState(nameof(MonsterRangedAttack));
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