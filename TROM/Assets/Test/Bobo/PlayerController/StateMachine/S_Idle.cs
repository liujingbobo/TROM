using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControllerTest
{
    public class S_Idle : IState
    {
        public override void StateEnter()
        {
            // TODO: Play Anim
        }
        public override void StateUpdate()
        {
            // TODO: 
        }
    
        public override void OnMove()
        {
            sm.Switch(FSM.PlayerState.Move);
        }

        public override void OnJump()
        {
            sm.Switch(FSM.PlayerState.Jump);
        }
    }
}
