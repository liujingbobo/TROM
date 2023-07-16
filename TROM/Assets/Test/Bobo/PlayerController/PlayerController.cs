using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllerTest
{
    public class PlayerController : MonoBehaviour
    {
        public bool grounded;
        
        [SerializeField] private LayerMask groundLayer;

        private void OnCollisionEnter2D(Collision2D col)
        {
            var targetLayer = 1 << col.gameObject.layer;
            if (targetLayer !=groundLayer) return;
            grounded = true;
            print("Touch Ground!!");
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var targetLayer = 1 << other.gameObject.layer;
            if (targetLayer !=groundLayer) return;
            grounded = false;
            print("Leave Ground!!");
        }
    }
}
