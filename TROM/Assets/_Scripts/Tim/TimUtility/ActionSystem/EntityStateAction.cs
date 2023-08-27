using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TimUtility;
using UnityEngine;

public class EntityStateAction : EntityAction<GameEntity>, IState
{
    public EntityActionStateMachine actionStateMachine;
    public ActionType actionType;
    public ActionPriorityType priorityType;
    public SameActionResolveType sameActionResolveType;
    public SamePriorityActionResolveType samePriorityActionResolveType;
    
    public float animationFinishTime;
    
    
    private Countdown _finishCountDown = new Countdown(1);
    protected override void OnActionStart()
    {
        _finishCountDown.countdownTime = animationFinishTime;
        _finishCountDown.Flush();
    }

    protected virtual void Update()
    {
        if (actionType == ActionType.TimedToDefaultOnComplete &&
            actionState == EntityActionState.InProgress &&
            _finishCountDown.IsCountdownOver())
        {
            StopAction(EntityActionStopReason.Completed);
        }
    }

    protected override void OnActionStop(EntityActionStopReason reason)
    {
        if (reason == EntityActionStopReason.Completed)
        {
            if (actionType == ActionType.TimedToDefaultOnComplete || actionType == ActionType.ToDefaultOnComplete)
            {
                actionStateMachine.TryPlayDefaultAction();
            }
        }

        if (reason == EntityActionStopReason.Interrupted)
        {
            if(showLog) Debug.Log(GetType().ToString() + " Interrupted");
        }
    }
    
    public void OnStateEnter()
    {
        StartAction();
    }

    public void OnStateUpdate()
    {
    }

    public void OnStateExit()
    {
    }
    
    [Button]
    public void SwitchToActionState()
    {
        actionStateMachine.TrySwitchAction(GetType().Name);
    }
    [Button]
    public void TestStartAction()
    {
        StartAction();
    }
    [Button]
    public void TestCompleteAction()
    {
        StopAction(EntityActionStopReason.Completed);
    }
    [Button]
    public void TestInterruptAction()
    {
        StopAction(EntityActionStopReason.Interrupted);
    }
}

public enum ActionType
{
    TimedToDefaultOnComplete,
    ToDefaultOnComplete,
    PlayLoop,
}

public enum ActionPriorityType
{
    Default,
    HighPriorityAction,
    ReactiveAction,
}
public enum SameActionResolveType
{
    Restart,
    Ignore,
}
public enum SamePriorityActionResolveType
{
    PlayOther,
    Ignore,
}