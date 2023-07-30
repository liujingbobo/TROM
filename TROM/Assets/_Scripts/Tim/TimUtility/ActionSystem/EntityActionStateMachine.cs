using System;
using System.Collections;
using System.Collections.Generic;
using TimUtility;
using UnityEngine;

public class EntityActionStateMachine: MonoBehaviour
{
    public string debugCurrentState = null;
    protected StateMachine<string, EntityStateAction> StateMachine;

    public EntityStateAction defaultAction;
    public List<EntityStateAction> actionList;
    public void Awake()
    {
        StateMachine = new StateMachine<string, EntityStateAction>();
        foreach (var stateAction in actionList)
        {
            StateMachine.AddState(stateAction.GetType().Name, stateAction);
        }
        StateMachine.SwitchToState(defaultAction.GetType().Name);
    }

    private void Update()
    {
        debugCurrentState = StateMachine.currentStateKey;
    }

    public EntityStateAction GetState(string stateName)
    {
        return StateMachine.GetState(stateName);
    }
    public bool TrySwitchAction(string actionStateKey)
    {
        var currentActionState = StateMachine.currentState;
        var targetActionState = StateMachine.GetState(actionStateKey);
        
        bool shouldSwitch = true;
        
        if (targetActionState.priorityType < currentActionState.priorityType) shouldSwitch = false;
        else if (targetActionState.priorityType == currentActionState.priorityType)
        {
            if (currentActionState == targetActionState)
            {
                switch (currentActionState.sameActionResolveType)
                {
                    case SameActionResolveType.Restart:
                        shouldSwitch = true;
                        break;
                    case SameActionResolveType.Ignore:
                        shouldSwitch = false;
                        break;
                }
            }
            else
            {
                switch (currentActionState.samePriorityActionResolveType)
                {
                    case SamePriorityActionResolveType.PlayOther:
                        shouldSwitch = true;
                        break;
                    case SamePriorityActionResolveType.Ignore:
                        shouldSwitch = false;
                        break;
                }
            }
        }
        
        if (shouldSwitch)
        {
            StateMachine.currentState?.StopAction(EntityActionStopReason.Interrupted);
            StateMachine.SwitchToState(actionStateKey);
        }
        return shouldSwitch;
    }

    public void TryPlayDefaultAction()
    {
        StateMachine.SwitchToState(defaultAction.GetType().Name);

    }
}
