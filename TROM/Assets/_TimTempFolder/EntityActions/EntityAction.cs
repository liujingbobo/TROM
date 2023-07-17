using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EntityAction<TEntity> : MonoBehaviour
{
    public TEntity fromEntity;
    public EntityActionState state;

    public event Action OnActionStarting;
    public event Action OnActionStarted;
    
    public event Action<EntityActionState, EntityActionStopReason> OnActionStopping;
    public event Action<EntityActionState, EntityActionStopReason> OnActionStopped;
    
    public virtual void Init(TEntity fromEntity)
    {
        this.fromEntity = fromEntity;
    }

    public void StartAction()
    {
        state = EntityActionState.InProgress;
        OnActionStarting?.Invoke();
        OnActionStart();
        OnActionStarted?.Invoke();
    }
    
    public void StopAction(EntityActionStopReason reason)
    {
        //check current state, if its in progress, handle base on reason
        if (state == EntityActionState.InProgress)
        {
            if (reason == EntityActionStopReason.Completed)
            {
                state = EntityActionState.Succeeded;
            }
            else
            {
                state = EntityActionState.Failed;
            }
            OnActionStopping?.Invoke(state, reason);
            OnActionStop(reason);
            OnActionStopped?.Invoke(state, reason);
        }
    }
    
    protected abstract void OnActionStart();
    protected abstract void OnActionStop(EntityActionStopReason reason);
    
    public virtual EntityActionState CheckState()
    {
        return state;
    }
}

public enum EntityActionState
{
    Invalid,
    InProgress,
    Failed,
    Succeeded
}

public enum EntityActionStopReason
{
    Completed,
    Interrupted,
}