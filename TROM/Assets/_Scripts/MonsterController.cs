using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonsterController : EntityActionStateMachine
{
    [Button]
    public void MoveTo(Vector2 position)
    {
        var key = nameof(MonsterMove);
        var moveAction = (MonsterMove) StateMachine.GetState(key);
        moveAction.position = position;
        if (StateMachine.GetCurrentStateKey() != key)
        {
            TryPlayAction(nameof(MonsterMove));
        }
    }
    
    [Button]
    public void AttackAt(Vector2 direction)
    {
        var action = (MonsterAttack) StateMachine.GetState(nameof(MonsterAttack));
        action.direction = direction;
        TryPlayAction(nameof(MonsterAttack));
    }
    
    [Button]
    public void SetIdle()
    {
        TryPlayAction(nameof(MonsterIdle));
    }
}
