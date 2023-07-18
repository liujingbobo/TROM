using System;
using UnityEngine;

public class GameEntityStateMachine : MonoStateMachine<GameEntity, string>
{
    public static GameEntityStateMachine CreateStateMachine(Transform parent,
        GameEntity context)
    {
        var GO = new GameObject("GameEntityMonoStateMachine");
        var sm = GO.AddComponent<GameEntityStateMachine>();
        sm.transform.SetParent(parent);
        sm.Initialize(context);
        //sm.LogDebug = true;
        return sm;
    }
    
    public TState CreateState<TState>(string stateKey) where TState : GameEntityState
    {
        var GO = new GameObject($"State_{stateKey}");
        var s = GO.AddComponent<TState>();
        s.transform.SetParent(transform);
        AddState(stateKey, s);
        return s;
    }

    public GameEntityState AddRuntimeState(
        string stateKey,
        Action enterAction = null,
        Action updateAction = null,
        Action endAction = null
    )
    {
        var s = CreateState<RuntimeGameEntityState>(stateKey);
        s.EnterAction += enterAction;
        s.UpdateAction += updateAction;
        s.EndAction += endAction;
        return s;
    }
}
public class GameEntityState : MonoState<GameEntity, string>
{
    protected GameEntity entity
    {
        get => context;
    }
}

public class RuntimeGameEntityState : GameEntityState
{
    public Action EnterAction = null;
    public Action UpdateAction = null;
    public Action EndAction = null;

    public override void EnterState(string fromState)
    {
        base.EnterState(fromState);
        EnterAction?.Invoke();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        UpdateAction?.Invoke();
    }

    public override void ExitState(string toState)
    {
        base.ExitState(toState);
        EndAction?.Invoke();
    }
}