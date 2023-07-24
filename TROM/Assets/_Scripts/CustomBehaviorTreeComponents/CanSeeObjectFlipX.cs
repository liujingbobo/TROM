using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[TaskDescription("Check to see if the any objects are within sight of the agent.")]
[TaskCategory("Movement")]
[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
public class CanSeeObjectFlipX : CanSeeObject
{
    public bool spriteFaceRight = true;
    public SpriteRenderer entitySpriteRenderer;
    public float angleOffset;
    public SharedTransform memoryTransform;
    
    public override void OnAwake()
    {
        var entity = transform.GetComponent<GameEntity>();
        if (entity == null) return;
        entitySpriteRenderer = entity.spriteRenderer;
    }
    
    public override TaskStatus OnUpdate()
    {
        if (entitySpriteRenderer == null) return TaskStatus.Failure;
            
        if (spriteFaceRight)
        {
            //sprite initially facing right, if flipped X, should be negative offset
            if(entitySpriteRenderer.flipX) angleOffset2D.Value = -angleOffset;
            else angleOffset2D.Value = angleOffset;
        }
        else
        {
            //sprite initially facing left, if flipped X, should be positive offset
            if(entitySpriteRenderer.flipX) angleOffset2D.Value = angleOffset;
            else angleOffset2D.Value = -angleOffset;
        }
        var result = base.OnUpdate();

        if(result == TaskStatus.Success) memoryTransform.Value.position = returnedObject.Value.transform.position;
        return result;
    }
}