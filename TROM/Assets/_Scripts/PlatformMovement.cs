using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using Pathfinding;
using Sirenix.OdinInspector;
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

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;


    [Button]
    public void TriggerPath()
    {
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }
    
    public void Start()
    {
        seeker = GetComponent<Seeker>();

        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        //seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public Vector3 lastUpdatePosition = Vector3.zero;
    public void FixedUpdate()
    {
        
    }

    public void Update()
    {
        if (path == null)
        {
            return;
        }
        reachedEndOfPath = false;
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    monsterController.SetIdle();
                    reachedEndOfPath = true;
                    path = null;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        if (path != null)
        {
            var target = path.vectorPath[currentWaypoint];
            monsterController.MoveTo(target);
        }
    }
}