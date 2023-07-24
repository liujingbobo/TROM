using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public abstract class IState : MonoBehaviour
    {
        protected FSM sm;
        public virtual IState Init(FSM sm)
        {
            this.sm = sm;
            return this;
        }
        public virtual void StateEnter(FSM.PlayerState preState){}
        public virtual void StateUpdate(){}
        public virtual void StateLateUpdate() { }
        public virtual void StateFixedUpdate(){}
        public virtual void StateExit(){}
        public virtual void OnMove(InputAction.CallbackContext context){}
        public virtual void OnInteract(InputAction.CallbackContext context){}
        public virtual void OnJump(InputAction.CallbackContext context){}
    }
}
