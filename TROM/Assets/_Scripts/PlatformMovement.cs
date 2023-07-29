using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using Pathfinding;
using Pathfinding.Util;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform target;
    public MonsterController monsterController;
    public MonsterMove monsterMove;
    public MonsterJump monsterJump;
    public float jumpAngleThreshold;
    public bool needToJump;

    public Transform targetPosition;

    public Seeker seeker;
    public bool waitingPathReturn;
    public List<Vector3> vectorPath;

    public float speed = 2;

    public float pickNextWaypointDist = 1;
    public float endReachedDistance = 1;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public Vector3 lastUpdatePosition;
    protected PathInterpolator interpolator = new PathInterpolator();
    public IMovementPlane movementPlane;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
    }
    
    public void Update()
    {
        if (!waitingPathReturn)
        {
            TriggerPath();
        }
    }

    public void FixedUpdate()
    {
        if (vectorPath == null)
        {
            return;
        }
        if (currentWaypoint >= vectorPath.Count)
        {
            return;
        }
        /*var currentPosition = transform.position;

        // Update which point we are moving towards
        interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);
        var steeringTarget = interpolator.valid ? interpolator.position : currentPosition;
        var dir = movementPlane.ToPlane(steeringTarget - currentPosition);
        // Calculate the distance to the end of the path
        float distanceToEnd = dir.magnitude + Mathf.Max(0, interpolator.remainingDistance);
        // Check if we have reached the target
        var prevTargetReached = reachedEndOfPath;
        reachedEndOfPath = distanceToEnd <= endReachedDistance && interpolator.valid;
        if (!prevTargetReached && reachedEndOfPath)
        {
            monsterController.SetIdle();
            vectorPath = null;
            interpolator.SetPath(null);
            return;
        }
        monsterController.MoveTo(steeringTarget);*/
        
        reachedEndOfPath = false;
        float distanceToWaypoint;
        while (true)
        {
            var currentPosition = transform.position;
            //check if current position
            distanceToWaypoint = Vector3.Distance( currentPosition, vectorPath[currentWaypoint]);
            if (distanceToWaypoint < pickNextWaypointDist)
            {
                if (currentWaypoint + 1 < vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    monsterController.SetIdle();
                    reachedEndOfPath = true;
                    vectorPath = null;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        if (vectorPath != null)
        {
            var target = vectorPath[currentWaypoint];
            monsterController.MoveTo(target);
        }
        lastUpdatePosition = transform.position;
    }

    [Button]
    public void TriggerPath()
    {
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        waitingPathReturn = true;
    }

    public void OnPathComplete(Path p)
    {
        waitingPathReturn = false;
        if (p.error)
        {
            vectorPath = null;
            return;
        }

        var newVectorPath = p.vectorPath;
        //if we already have a path and the starting point aligned with in our last point and current point
        // discard the start point
        /*if (vectorPath != null && currentWaypoint < vectorPath.Count)
        {
            if (newVectorPath.Count == 1) newVectorPath.Add(newVectorPath[^1]);
            var lastPoint = currentWaypoint >= 1
                ? vectorPath[currentWaypoint - 1] : vectorPath[currentWaypoint];
            var nextPoint = vectorPath[currentWaypoint];
            var bound = new Bounds(new Vector3((nextPoint.x + lastPoint.x) / 2, (nextPoint.y + lastPoint.y) / 2, 0),
                new Vector3((nextPoint.x - lastPoint.x), (nextPoint.y - lastPoint.y), 0)
            );
            if (bound.Contains(transform.position) && newVectorPath.Count>1)
            {
                newVectorPath.RemoveAt(0);
            }
        }*/
        vectorPath = newVectorPath;
        currentWaypoint = 0;
        
        /*
        ABPath p = newPath as ABPath;
        if (p == null) throw new System.Exception("This function only handles ABPaths, do not use special path types");

        waitingPathReturn = false;
        
        // Increase the reference count on the new path.
        // This is used for object pooling to reduce allocations.
        p.Claim(this);

        // Path couldn't be calculated of some reason.
        // More info in p.errorLog (debug string)
        if (p.error) {
            p.Release(this);
            path = null;
            return;
        }

        // Release the previous path.
        if (path != null) path.Release(this);

        // Replace the old path
        path = p;

        // Make sure the path contains at least 2 points
        if (path.vectorPath.Count == 1) path.vectorPath.Add(path.vectorPath[0]);
        interpolator.SetPath(path.vectorPath);
            
        movementPlane = new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 270, 90), Vector3.one));
        // Reset some variables
        reachedEndOfPath = false;

        var currentPosition = transform.position;
        // Simulate movement from the point where the path was requested
        // to where we are right now. This reduces the risk that the agent
        // gets confused because the first point in the path is far away
        // from the current position (possibly behind it which could cause
        // the agent to turn around, and that looks pretty bad).
        interpolator.MoveToLocallyClosestPoint((currentPosition + p.originalStartPoint) * 0.5f);
        interpolator.MoveToLocallyClosestPoint(currentPosition);

        // Update which point we are moving towards.
        // Note that we need to do this here because otherwise the remainingDistance field might be incorrect for 1 frame.
        // (due to interpolator.remainingDistance being incorrect).
        interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);

        var distanceToEnd = interpolator.remainingDistance + movementPlane.ToPlane(interpolator.position - currentPosition).magnitude;
        if (distanceToEnd <= endReachedDistance) {
            reachedEndOfPath = true;
        }*/
    }

    private void OnDrawGizmos()
    {
        if (vectorPath != null)
        {
            foreach (var VARIABLE in vectorPath)
            {
                DebugExtensions.DrawMarker(VARIABLE, 1f,Color.red,0.02f);
            }
        }
    }
}