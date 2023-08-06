using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonsterController : EntityActionStateMachine
{
    [Button]
    public void GetStaggered()
    {
        TrySwitchAction(nameof(MonsterStagger));
    }
    
    [Button]
    public void MoveTo(Vector2 position)
    {
        var key = nameof(MonsterMove);
        var moveAction = (MonsterMove) StateMachine.GetState(key);
        moveAction.position = position;
        if (StateMachine.GetCurrentStateKey() != key)
        {
            TrySwitchAction(nameof(MonsterMove));
        }
    }
    
    [Button]
    public void AttackAt(Vector3 targetPoint, Action<EntityActionState, EntityActionStopReason> callback = null)
    {
        var action = (MonsterAttack) StateMachine.GetState(nameof(MonsterAttack));
        action.targetPoint = targetPoint;
        TrySwitchAction(nameof(MonsterAttack));
        if(callback != null) action.OnActionStopped += callback;
    }
    
    [Button]
    public void RangeAttackAt(Vector3 targetPoint, Action<EntityActionState, EntityActionStopReason> callback = null)
    {
        var action = (MonsterRangedAttack) StateMachine.GetState(nameof(MonsterRangedAttack));
        action.targetPoint = targetPoint;
        TrySwitchAction(nameof(MonsterRangedAttack));
        if(callback != null) action.OnActionStopped += callback;
    }
    
    [Button]
    public void SetIdle()
    {
        TrySwitchAction(nameof(MonsterIdle));
    }
}
