using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerDetection : MonoBehaviour
{
    public CustomColliderDetector upperHangDetector;
    public CustomColliderDetector downHangDetector;
    public CustomColliderDetector ladderDetector;


    [BoxGroup("GroundDetection"), SerializeField]
    private float groundRaycastOffsetX;

    [BoxGroup("GroundDetection"), SerializeField]
    private float groundRaycastOffsetY;

    [BoxGroup("GroundDetection"), SerializeField]
    private float groundRaycastLength;

    [BoxGroup("GroundDetection"), SerializeField]
    public RaycastHit2D LeftGroundedHit2D;

    [BoxGroup("GroundDetection"), SerializeField]
    public RaycastHit2D RightGroundedHit2D;

    [BoxGroup("GroundDetection"), SerializeField]
    private LayerMask groundLayer;


    [BoxGroup("MidGroundDetection"), SerializeField]
    private float midGroundRaycastLength;

    [BoxGroup("MidGroundDetection"), SerializeField]
    public RaycastHit2D MidGroundRaycastHit2D;
    
    public bool isGrounded;

    public Vector2 slopeNormal;

    public void ResetGrounded()
    {
        var position = transform.position;

        LeftGroundedHit2D =
            Physics2D.Raycast(position.xy() + new Vector2(-groundRaycastOffsetX, groundRaycastOffsetY),
                Vector2.down, groundRaycastLength, groundLayer);

        RightGroundedHit2D =
            Physics2D.Raycast(position.xy() + new Vector2(groundRaycastOffsetX, groundRaycastOffsetY),
                Vector2.down, groundRaycastLength, groundLayer);

        MidGroundRaycastHit2D = Physics2D.Raycast(position.xy() + new Vector2(0, groundRaycastOffsetY),
            Vector2.down, midGroundRaycastLength, groundLayer);

        isGrounded = LeftGroundedHit2D || RightGroundedHit2D;

        if (MidGroundRaycastHit2D)
        {
            slopeNormal = MidGroundRaycastHit2D.normal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        var position = transform.position;
        
        Gizmos.DrawLine(new Vector2(position.x -groundRaycastOffsetX,position.y + groundRaycastOffsetY),
            new Vector2(position.x -groundRaycastOffsetX,position.y+ groundRaycastOffsetY) - new Vector2(0, groundRaycastLength));     
        
        Gizmos.DrawLine(new Vector2(position.x + groundRaycastOffsetX,position.y+ groundRaycastOffsetY),
            new Vector2(position.x +groundRaycastOffsetX,position.y+ groundRaycastOffsetY) - new Vector2(0, groundRaycastLength));    
        
        Gizmos.DrawLine(new Vector2(position.x,position.y+ groundRaycastOffsetY),
            new Vector2(position.x,position.y+ groundRaycastOffsetY) - new Vector2(0, groundRaycastLength));
    }
}