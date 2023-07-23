using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Movement;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

[RequireComponent(typeof(Seeker))]
public class PlatformAStarMovement : MonoBehaviour
{
    public Seeker seeker;
    public Transform targetPosition;

    public Path path;
    private int currentWaypoint = 0;
    public float speed = 2;
    public float switchWayPointDistance;
    public float jumpPointThreshold;
    
    public bool reachedEndOfPath;
    public bool reachedJumpStartPoint;
    public bool landedJump;
    
    public Vector2 jumpStartPoint;
    public Vector2 jumpEndPoint;
    public float jumpStartTime;
    public void Start () {
        if(seeker == null) seeker = GetComponent<Seeker>();
    }

    [Button]
    public void ChaseToTargetTransform()
    {
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete (Path p) {
        if (p.error)
        {
            Debug.LogError("OnPathComplete Error: " + p.errorLog);
        }
        else
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
            StopAllCoroutines();
            StartCoroutine(PathMovement());
        }
    }
    
    IEnumerator PathMovement()
    {
        reachedEndOfPath = false;
        reachedJumpStartPoint = false;
        
        while (!reachedEndOfPath)
        {
            float distanceToWaypoint = Vector3.Magnitude(transform.position - path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < switchWayPointDistance) {
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    reachedEndOfPath = true;
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
                Vector3 velocity = dir * speed;
                transform.position += velocity * Time.deltaTime;
            }
            
            yield return null;
        }
        
        Debug.Log($"Path Reached");
    }
}
