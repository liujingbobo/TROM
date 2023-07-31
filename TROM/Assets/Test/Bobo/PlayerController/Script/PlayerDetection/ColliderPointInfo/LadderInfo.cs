using System.Collections;
using System.Collections.Generic;
using PlayerControllerTest;
using UnityEngine;

public class LadderInfo : MonoBehaviour
{
    public Collider2D topCollider;

    public Collider2D bottomCollider;

    public List<PlayerDirection> validDirection;

    public Transform topPoint; 

    public Transform climbMaxPoint;

    public Transform bottomPoint;
}
