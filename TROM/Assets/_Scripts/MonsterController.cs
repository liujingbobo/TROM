using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonsterController : EntityActionStateMachine
{
    [Button]
    public void MoveTo(Vector2 direction)
    {
        var key = nameof(MonsterHorizontalMove);
        var moveAction = (MonsterHorizontalMove) StateMachine.GetState(key);
        moveAction.direction = direction;
        if (StateMachine.GetCurrentStateKey() != key)
        {
            TryPlayAction(nameof(MonsterHorizontalMove));
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
