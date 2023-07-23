using System;
using System.Collections;
using System.Collections.Generic;
using TimUtility;
using UnityEngine;

public class EntityActionStateMachine: MonoBehaviour
{
    public string debugCurrentState = null;
    private StateMachine<string, EntityStateAction> stateMachine;

    public EntityStateAction defaultAction;
    public List<EntityStateAction> actionList;
    public void Awake()
    {
        stateMachine = new StateMachine<string, EntityStateAction>();
        foreach (var stateAction in actionList)
        {
            stateMachine.AddState(stateAction.GetType().Name, stateAction);
        }
        stateMachine.SwitchToState(defaultAction.GetType().Name);
    }

    private void Update()
    {
        debugCurrentState = stateMachine.currentStateKey;
    }

    public void SwitchToState(string actionStateKey)
    {
        stateMachine.currentState?.StopAction(EntityActionStopReason.Interrupted);
        stateMachine.SwitchToState(actionStateKey);
        
    }
    
    public void SwitchToDefaultAction()
    {
        stateMachine.SwitchToState(defaultAction.GetType().Name);

    }
}
