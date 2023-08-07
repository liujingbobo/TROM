using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class S_Hit : IState
    {
        // Move
        [BoxGroup("Move")]public float recoverTime = 1f;
        private Countdown stiffEndCountdown = new Countdown(1);
        private Rigidbody2D TargetRb2D => sm.targetRb2D;
        public override void StateEnter(PlayerState preState, params object[] objects)
        {
            sm.ForceFixPosition();
            sm.PlayAnim(AnimationType.Hit);
            TargetRb2D.velocity = Vector2.zero;
            stiffEndCountdown.countdownTime = recoverTime;
            stiffEndCountdown.Flush();

            var info = objects[0] as AttackReleaseInfo;
            var direction = info.fromEntity.transform.position.x < sm.transform.position.x
                ? PlayerDirection.Left
                : PlayerDirection.Right;
            sm.SetDirection(direction);
        }
        
        public override void OnMove(InputAction.CallbackContext context)
        {
        }
        
        public override void OnJump(InputAction.CallbackContext context)
        {
        }
        
        public override void OnAttack(InputAction.CallbackContext context)
        {
        }

        public override void StateUpdate()
        {
            base.StateUpdate();
            if (stiffEndCountdown.IsCountdownOver())
            {
                sm.Switch(PlayerState.Idle);
                return;
            }
        }
    }
}
