using System;
using UnityEngine;
using System.Collections.Generic;

namespace TimUtility
{
    public class StateMachine<TStateKey>
    {
        public TStateKey currentStateKey;
        public IState currentState;
        private Dictionary<TStateKey, IState> states;

        public StateMachine()
        {
            states = new Dictionary<TStateKey, IState>();
        }

        public TStateKey GetCurrentStateKey()
        {
            return currentStateKey;
        }

        public IState GetCurrentState()
        {
            return currentState;
        }

        public IState GetState(TStateKey key)
        {
            if (states.ContainsKey(key))
            {
                return states[key];
            }
            else
            {
                Debug.LogError($"State Key:{key} is not found in the state machine.");
                return default;
            }
        }
        
        public void AddState(TStateKey key, IState state)
        {
            if (!states.ContainsKey(key))
            {
                states.Add(key, state);
            }
            else
            {
                Debug.LogError($"State Key:{key} is already added to the state machine.");
            }
        }

        public void RemoveState(TStateKey key)
        {
            if (states.ContainsKey(key))
            {
                states.Remove(key);
            }
            else
            {
                Debug.LogError($"State Key:{key} is not found in the state machine.");
            }
        }
        
        public void SwitchToState(TStateKey key)
        {
            if (states.TryGetValue(key, out IState state))
            {
                currentState?.OnStateExit();

                currentStateKey = key;
                currentState = state;
                
                currentState.OnStateEnter();
            }else
            {
                Debug.LogError($"State Key:{key} not found in the state machine.");
            }
        }

        public void ExecuteStateUpdate()
        {
            currentState?.OnStateUpdate();
        }

        public StateMachine<TStateKey> AddRuntimeState(TStateKey stateKey,
            Action onStateStart, Action onStateUpdate, Action onStateExit)
        {
            var newState = new RuntimeState(onStateStart, onStateUpdate, onStateExit) as IState;
            AddState(stateKey, newState);
            return this;
        }
    }

    public interface IState
    {
        void OnStateEnter();
        void OnStateUpdate();
        void OnStateExit();
    }

    public class RuntimeState : IState
    {
        private Action OnStateStartCallBack;
        private Action OnStateUpdateCallBack;
        private Action OnStateExitCallBack;

        public RuntimeState(Action onStateStart, Action onStateUpdate, Action onStateExit)
        {
            OnStateStartCallBack = onStateStart;
            OnStateUpdateCallBack = onStateUpdate;
            OnStateExitCallBack = onStateExit;
        }

        public void OnStateEnter()
        {
            OnStateStartCallBack?.Invoke();
        }
        public void OnStateUpdate()
        {
            OnStateUpdateCallBack?.Invoke();
        }
        public void OnStateExit()
        {
            OnStateExitCallBack?.Invoke();
        }
    }
}