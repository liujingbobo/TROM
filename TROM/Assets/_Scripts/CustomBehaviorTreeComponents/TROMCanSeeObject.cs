using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskDescription("Check to see if the any objects are within sight of the agent. and remember its last seen point in chase point")]
[TaskCategory("TROM")]
[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
public class TROMCanSeeObject : CanSeeObject
{
    public SharedBehaviour gameEntityBehaviour;
    public SharedGameObject seenObject;
    public SharedVector3 lastSeenPoint;

    public float checkGroundRaycastDistance;
    public LayerMask checkGroundCollideLayer;
    private GameEntity _gameEntity;
    public override void OnAwake()
    {
        _gameEntity = gameEntityBehaviour.GetValue() as GameEntity;
    }
    
    public override TaskStatus OnUpdate()
    {
        if (_gameEntity == null) return TaskStatus.Failure;
        
        if (_gameEntity.IsFacingRight)
        {
            angleOffset2D.Value = Mathf.Abs(angleOffset2D.Value);
        }
        else
        {
            angleOffset2D.Value = -Mathf.Abs(angleOffset2D.Value);
        }
        var result = base.OnUpdate();

        if (result == TaskStatus.Success)
        {
            seenObject.Value = returnedObject.Value;
            RaycastHit2D hit = Physics2D.Raycast(seenObject.Value.transform.position, 
                Vector2.down, checkGroundRaycastDistance, checkGroundCollideLayer);
            // Check if the raycast hit the ground (hit.collider != null) and if the distance to the ground is less than or equal to the raycast distance.
            if (hit.collider != null && hit.distance <= checkGroundRaycastDistance)
            {
                lastSeenPoint.Value = hit.point;
            }
            else
            {
                lastSeenPoint.Value = Vector3.zero;
            }
        }
        return result;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastSeenPoint.Value, 0.1f);
    }
}