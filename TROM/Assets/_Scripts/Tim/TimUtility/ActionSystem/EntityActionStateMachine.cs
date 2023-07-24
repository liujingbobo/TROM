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

    public void TryPlayAction(string actionStateKey)
    {
        StateMachine.currentState?.StopAction(EntityActionStopReason.Interrupted);
        StateMachine.SwitchToState(actionStateKey);
    }
    
    public void TryPlayDefaultAction()
    {
        StateMachine.SwitchToState(defaultAction.GetType().Name);

    }
}
