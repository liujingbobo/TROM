using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Movement;
using Pathfinding;
using Pathfinding.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

[RequireComponent(typeof(Seeker))]
public class PlatformAStarMovement : MonoBehaviour
{
    public Seeker seeker;
    public Transform targetTransform;
    public MonsterController monsterController;
    
    public Path path;
    public Vector3 pathWayPoints;
    public int currentWaypoint = 0;

    public float pickNextWaypointDist;
    public float jumpPointThreshold;
    
    public bool reachedEndOfPath;
    public float endReachedDistance;
    
    public bool reachedJumpStartPoint;
    public bool landedJump;
    
    public Vector2 jumpStartPoint;
    public Vector2 jumpEndPoint;
    public float jumpStartTime;

    public IMovementPlane movementPlane = GraphTransform.identityTransform;
    protected bool waitingForPathCalculation = false;
    protected PathInterpolator interpolator = new PathInterpolator();

    public ProcessState processState;
    public enum ProcessState
    {
        WaitingDestination,
        WaitingPath,
        
    }
    public void Start () {
        if(seeker == null) seeker = GetComponent<Seeker>();
    }

    [Button]
    public void ChaseToTargetTransform()
    {
        seeker.StartPath(transform.position, targetTransform.position, OnPathComplete);
        reachedEndOfPath = false;
    }

    public void OnPathComplete (Path newPath) {
        /*if (p.error)
        {
            Debug.LogError("OnPathComplete Error: " + p.errorLog);
        }
        else
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
            StopAllCoroutines();
            foreach (var waypoint in path.vectorPath)
            {
                DebugExtensions.DrawMarker(waypoint, 1, Color.red, 1);
            }
            //StartCoroutine(PathMovement());
        }*/
        ABPath p = newPath as ABPath;

        if (p == null) throw new System.Exception("This function only handles ABPaths, do not use special path types");

			waitingForPathCalculation = false;

			// Increase the reference count on the new path.
			// This is used for object pooling to reduce allocations.
			p.Claim(this);

			// Path couldn't be calculated of some reason.
			// More info in p.errorLog (debug string)
			if (p.error) {
				p.Release(this);
				return;
			}

			// Release the previous path.
			if (path != null) path.Release(this);

			// Replace the old path
			path = p;
			
			// Make sure the path contains at least 2 points
			if (path.vectorPath.Count == 1) path.vectorPath.Add(path.vectorPath[0]);
			interpolator.SetPath(path.vectorPath);

			var graph = path.path.Count > 0 ? AstarData.GetGraph(path.path[0]) as ITransformedGraph : null;
			movementPlane = graph != null ? graph.transform : new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 270, 90), Vector3.one));

			// Reset some variables
			reachedEndOfPath = false;

			var position = transform.position;
			// Simulate movement from the point where the path was requested
			// to where we are right now. This reduces the risk that the agent
			// gets confused because the first point in the path is far away
			// from the current position (possibly behind it which could cause
			// the agent to turn around, and that looks pretty bad).
			interpolator.MoveToLocallyClosestPoint((position + p.originalStartPoint) * 0.5f);
			interpolator.MoveToLocallyClosestPoint(position);

			// Update which point we are moving towards.
			// Note that we need to do this here because otherwise the remainingDistance field might be incorrect for 1 frame.
			// (due to interpolator.remainingDistance being incorrect).
			interpolator.MoveToCircleIntersection2D(position, pickNextWaypointDist, movementPlane);

			var distanceToEnd = remainingDistance;
			if (distanceToEnd <= endReachedDistance) {
				reachedEndOfPath = true;
			}
    }
    public float remainingDistance {
	    get {
		    return interpolator.valid ? 
			    interpolator.remainingDistance + movementPlane.ToPlane(interpolator.position - transform.position).magnitude : 
			    float.PositiveInfinity;
	    }
    }
    IEnumerator PathMovement()
    {
        reachedEndOfPath = false;

        var finalPosition = path.vectorPath[^1];
        while (!reachedEndOfPath)
        {
            if (Vector3.Magnitude(transform.position - finalPosition) < endReachedDistance)
            {
                reachedEndOfPath = true;
                break;
            }
            
            float distanceToWaypoint = Vector3.Magnitude(transform.position - path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < pickNextWaypointDist) {
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    reachedEndOfPath = true;
                    monsterController.SetIdle();
                    break;
                }
            }

            if (reachedJumpStartPoint)
            {
                var ratio = (Time.time - jumpStartTime) / 1f;
                if (ratio > 1) reachedJumpStartPoint = false;
                else transform.position = Vector2.Lerp(jumpStartPoint, jumpEndPoint, ratio);
                yield return null;
                continue;
            }
            
            //if waypoint over jumpPoint threshold, we need to jump to get there
            if ( path.vectorPath[currentWaypoint].y - transform.position.y > jumpPointThreshold)
            {
                reachedJumpStartPoint = true;
                jumpStartPoint = transform.position;
                jumpEndPoint = path.vectorPath[currentWaypoint];
                jumpStartTime = Time.time;
            }
            else // move directly towards
            {
                Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                monsterController.MoveTo(dir);
            }
            
            yield return null;
        }
        
        Debug.Log($"Path Reached");
    }
}
