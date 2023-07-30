using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Pathfinding;
using UnityEngine;

public class PlatformAILerp : AILerp
{
    public MonsterController monsterController;
    public float reachedEndPointDistance;


    public bool ReachedDestination
    {
        get
        {
            return !shouldRecalculatePath && reachedDestination;
        }
    }
    public void SetDestination(Vector3 point)
    {
        destination = point;
    }
    protected override void Update () {
        if (shouldRecalculatePath)
        {
            SearchPath();
        }
        if (canMove) {
            Vector3 nextPosition;
            Quaternion nextRotation;
            MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);

            if (!reachedDestination)
            {
                monsterController.MoveTo(nextPosition);
            }
        }
    }

    public override void OnTargetReached()
    {
        base.OnTargetReached();
        monsterController.SetIdle();
    }
}
