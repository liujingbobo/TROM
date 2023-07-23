using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EntityAction<TEntity> : MonoBehaviour
{
    public bool showLog = false;
    public TEntity fromEntity;
    public EntityActionState actionState;

    public event Action OnActionStarting;
    public event Action OnActionStarted;
    
    public event Action<EntityActionState, EntityActionStopReason> OnActionStopping;
    public event Action<EntityActionState, EntityActionStopReason> OnActionStopped;
    
    public virtual void Init(TEntity fromEntity)
    {
        this.fromEntity = fromEntity;
    }

    private void Awake()
    {
        actionState = EntityActionState.Invalid;
    }

    public void StartAction()
    {
        actionState = EntityActionState.InProgress;
        OnActionStarting?.Invoke();
        OnActionStart();
        OnActionStarted?.Invoke();
        if(showLog) Debug.Log($"{this.GetType().Name} Action Started");
    }
    
    public void StopAction(EntityActionStopReason reason)
    {
        //check current state, if its in progress, handle base on reason
        if (actionState == EntityActionState.InProgress)
        {
            if (reason == EntityActionStopReason.Completed)
            {
                actionState = EntityActionState.Completed;
            }
            else
            {
                actionState = EntityActionState.Interrupted;
            }
            OnActionStopping?.Invoke(actionState, reason);
            OnActionStop(reason);
            OnActionStopped?.Invoke(actionState, reason);
        }
        if(showLog) Debug.Log($"{this.GetType().Name} Action Started");

    }
    
    protected abstract void OnActionStart();
    protected abstract void OnActionStop(EntityActionStopReason reason);
    
    public virtual EntityActionState GetState()
    {
        return actionState;
    }
}

public enum EntityActionState
{
    Invalid,
    InProgress,
    Completed,
    Interrupted,
}

public enum EntityActionStopReason
{
    Completed,
    Interrupted,
}