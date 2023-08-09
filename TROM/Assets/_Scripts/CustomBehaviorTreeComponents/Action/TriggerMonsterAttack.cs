using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MonsterControl")]
public class TriggerMonsterAttack : Action
{
	public SharedBehaviour monsterControllerSB;
	private MonsterController mc;
	public SharedGameObject targetGO;
	public override void OnStart()
	{
		mc = monsterControllerSB.Value as MonsterController;
		mc.AttackAt(targetGO.Value.transform.position);
	}

	public override TaskStatus OnUpdate()
	{
		var action = (MonsterAttack)mc.GetState(nameof(MonsterAttack));
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