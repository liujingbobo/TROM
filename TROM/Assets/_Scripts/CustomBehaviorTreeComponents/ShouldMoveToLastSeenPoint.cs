using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("TROM")]
public class ShouldMoveToLastSeenPoint : Conditional
{
	public SharedBehaviour gameEntityBehaviour;
	private GameEntity _gameEntity;
	public SharedVector3 lastSeenPoint;

	public float arrivedDistance = 0.1f;
	public override void OnAwake()
	{
		_gameEntity = gameEntityBehaviour.GetValue() as GameEntity;
	}
	public override void OnStart()
	{
	}

	public override TaskStatus OnUpdate()
	{
		if (lastSeenPoint.Value == Vector3.zero) return TaskStatus.Failure;

		if ((_gameEntity.transform.position - lastSeenPoint.Value).magnitude < arrivedDistance)
			return TaskStatus.Failure;
		
		return TaskStatus.Success;
	}
}