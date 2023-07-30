using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Set Path finding destination of a PlatformAILerp Class")]
[TaskCategory("MonsterControl")]
public class SetMovementTargetTransform : Action
{
	public LayerMask collideLayer;
	public float raycastDistance;
	
	public SharedTransform targetTransform;
	public SharedTransform movementTargetTransform;

	private bool _foundPoint;
	public override void OnStart()
	{
		_foundPoint = false;
		RaycastHit2D hit = Physics2D.Raycast(targetTransform.Value.position, Vector2.down, raycastDistance, collideLayer);

		// Check if the raycast hit the ground (hit.collider != null) and if the distance to the ground is less than or equal to the raycast distance.
		if (hit.collider != null && hit.distance <= raycastDistance)
		{
			_foundPoint = true;
			movementTargetTransform.Value.position = hit.point;
		}
		else
		{
			_foundPoint = false;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (_foundPoint) return TaskStatus.Success;
		else return TaskStatus.Failure;
	}
}