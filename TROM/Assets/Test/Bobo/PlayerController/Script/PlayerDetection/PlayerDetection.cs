using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [BoxGroup("GroundDetection"), SerializeField]
    private LayerMask slopeLayer;

    [BoxGroup("GroundDetection"), SerializeField]
    private float verticalGapFromGround = 0.03f;

    [BoxGroup("MidGroundDetection"), SerializeField]
    private float midGroundRaycastLength;

    [BoxGroup("MidGroundDetection"), SerializeField]
    public RaycastHit2D MidGroundRaycastHit2D;

    public bool isGrounded;

    public RaycastHit2D GetActiveRaycast2D()
    {
        if (MidGroundRaycastHit2D) return MidGroundRaycastHit2D;
        if (LeftGroundedHit2D) return LeftGroundedHit2D;
        if (RightGroundedHit2D) return RightGroundedHit2D;
        return MidGroundRaycastHit2D;
    }

    public void GroundDetect()
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

        if (!MidGroundRaycastHit2D)
        {
            MidGroundRaycastHit2D = Physics2D.Raycast(position.xy() + new Vector2(0, groundRaycastOffsetY),
                Vector2.down, midGroundRaycastLength, slopeLayer);
        }

        isGrounded = LeftGroundedHit2D || RightGroundedHit2D;
    }

    public (Vector2 posDirection, float stickToGroundY) GetSlopeInfo(Vector2 input)
    {
        RaycastHit2D hit = default;

        if (LeftGroundedHit2D)
        {
            if (RightGroundedHit2D)
            {
                var leftPoint = LeftGroundedHit2D.point.y;
                var rightPoint = RightGroundedHit2D.point.y;
                
                if (leftPoint > rightPoint)
                {
                    hit = LeftGroundedHit2D;
                }else if (leftPoint == rightPoint)
                {
                    hit = input.x > 0 ? RightGroundedHit2D : LeftGroundedHit2D;
                }
                else
                {
                    hit = RightGroundedHit2D;
                }
            }
            else
            {
                hit = LeftGroundedHit2D;
            }
        }
        else if (RightGroundedHit2D)
        {
            hit = RightGroundedHit2D;
        }

        return (Vector3.ProjectOnPlane(Vector2.right, hit.normal), hit.point.y + verticalGapFromGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        var position = transform.position;

        Gizmos.DrawLine(new Vector2(position.x - groundRaycastOffsetX, position.y + groundRaycastOffsetY),
            new Vector2(position.x - groundRaycastOffsetX, position.y + groundRaycastOffsetY) -
            new Vector2(0, groundRaycastLength));

        Gizmos.DrawLine(new Vector2(position.x + groundRaycastOffsetX, position.y + groundRaycastOffsetY),
            new Vector2(position.x + groundRaycastOffsetX, position.y + groundRaycastOffsetY) -
            new Vector2(0, groundRaycastLength));

        Gizmos.DrawLine(new Vector2(position.x, position.y + groundRaycastOffsetY),
            new Vector2(position.x, position.y + groundRaycastOffsetY) - new Vector2(0, groundRaycastLength));
    }
}