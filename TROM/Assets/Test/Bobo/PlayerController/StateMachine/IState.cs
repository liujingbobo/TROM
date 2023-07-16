using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerTest
{
    public abstract class IState
    {
        protected FSM sm;
        
        public virtual IState Init(FSM sm)
        {
            this.sm = sm;
            return this;
        }
        public virtual void StateEnter(){}
        public virtual void StateUpdate(){}
        
        public virtual void StateFixedUpdate(){}
        public virtual void StateExit(){}
        public virtual void OnMove(){}
    
        public virtual void OnInteract(){}
        public virtual void OnJump(){}
    }
}
