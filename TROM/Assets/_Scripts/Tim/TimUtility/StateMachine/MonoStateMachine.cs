using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoStateMachine<TContext, TStateKey> : MonoBehaviour
{
    private MonoState<TContext, TStateKey> _currentState;
    private TContext context;
    private Dictionary<TStateKey, MonoState<TContext, TStateKey>> states = new Dictionary<TStateKey, MonoState<TContext, TStateKey>>();

    public bool LogDebug = false;

    [SerializeField]
    private TStateKey displayCurrentState;

    private void Update()
    {
        displayCurrentState = CurrentStateKey;
    }

    public void Initialize(TContext context)
    {
        this.context = context;
        states = new Dictionary<TStateKey, MonoState<TContext, TStateKey>>();
    }

    public void AddState(TStateKey key, MonoState<TContext, TStateKey> state)
    {
        if(!states.ContainsKey(key))
        {
            state.Initialize(key, this, context);
            states.Add(key, state);
        }
        else
        {
            Debug.LogError("State with the same key already exists.");
        }
    }

    public void RemoveState(TStateKey key)
    {
        if(states.ContainsKey(key))
        {
            states.Remove(key);
        }
        else
        {
            Debug.LogError("Trying to remove non-existing state.");
        }
    }
    public MonoState<TContext, TStateKey> GetState(TStateKey key)
    {
        if(states.ContainsKey(key))
        {
            return states[key];
        }
        return null;
    }
    public void StartAtState(TStateKey key)
    {
        if(states.ContainsKey(key))
        {
            _currentState = states[key];
            _currentState.EnterState(key);
        }
        else
        {
            Debug.LogError("Trying to start machine with non-existing state.");
        }
    }

    public void ExecuteStateUpdate()
    {
        if(_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    public void SwitchToState(TStateKey targetStateKey)
    {
        if(states.ContainsKey(targetStateKey))
        {
            TStateKey prevStateKey = default(TStateKey);
            if(_currentState != null)
            {
                _currentState.ExitState(targetStateKey);
                prevStateKey = _currentState.StateKey;
            }
            if(LogDebug) Debug.Log($"MonoStateMachine {_currentState} TransitionToState {states[targetStateKey]}");
            _currentState = states[targetStateKey];
            _currentState.EnterState(prevStateKey);
        }
        else
        {
            Debug.LogError("Trying to transition to non-existing state.");
        }
    }

    public MonoState<TContext, TStateKey> CurrentState
    {
        get => _currentState;
    }

    public TStateKey CurrentStateKey
    {
        get => _currentState.StateKey;
    }
}

public class MonoState<TContext, TStateKey> : MonoBehaviour
{
    public TStateKey StateKey { get; private set;}
    protected MonoStateMachine<TContext, TStateKey> stateMachine;
    protected TContext context;

    public void Initialize(TStateKey stateKey, MonoStateMachine<TContext, TStateKey> stateMachine, TContext context)
    {
        StateKey = stateKey;
        this.stateMachine = stateMachine;
        this.context = context;
    }
    
    public virtual void EnterState(TStateKey fromState) {}

    public virtual void UpdateState() {}
    
    public virtual void ExitState(TStateKey toState) {}
    
    public virtual void SwitchToState(TStateKey targetState)
    {
        stateMachine.SwitchToState(targetState);
    }
}