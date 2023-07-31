using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private float groundCheckHeight = 0.5f;

    [SerializeField] private bool grounded;

    private RaycastHit2D groundHit;

    public CustomColliderDetector upperHangDetector;
    public CustomColliderDetector downHangDetector;
    public CustomColliderDetector ladderDetector;
    
    public RaycastHit2D GroundHit => groundHit;
    public bool IsGrounded => grounded;
    
    private void FixedUpdate()
    {
        ResetGrounded();
    }

    public void ResetGrounded()
    {
        var newPosition = transform.position;
                
        groundHit = Physics2D.Raycast(new Vector2(newPosition.x, newPosition.y) + Vector2.up, Vector2.down, 1f + groundCheckHeight,
            LayerMask.GetMask($"Ground"));

        grounded = groundHit.collider != null;
    }
}
