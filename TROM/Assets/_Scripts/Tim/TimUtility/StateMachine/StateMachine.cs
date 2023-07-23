using UnityEngine;
using System.Collections.Generic;

namespace TimUtility
{
    public class StateMachine<TStateKey, TState> where TState : IState
    {
        public TStateKey currentStateKey;
        public TState currentState;
        private Dictionary<TStateKey, TState> states;

        public StateMachine()
        {
            states = new Dictionary<TStateKey, TState>();
        }

        public TStateKey GetCurrentStateKey()
        {
            return currentStateKey;
        }

        public TState GetCurrentState()
        {
            return currentState;
        }

        public void AddState(TStateKey key, TState state)
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
            if (states.TryGetValue(key, out TState state))
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
            currentState?.OnStateExecute();
        }
    }

    public interface IState
    {
        void OnStateEnter();
        void OnStateExecute();
        void OnStateExit();
    }
}