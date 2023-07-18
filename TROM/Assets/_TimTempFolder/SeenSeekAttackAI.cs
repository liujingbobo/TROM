using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenSeekAttackAI : MonoBehaviour
{
    public GameEntity fromEntity;
    
    public float seekRadius;
    public float attackDistance;
    public LayerMask layerMask;


    public Transform chaseTarget;
    
    private GameEntityStateMachine _gsm;

    public enum State
    {
        Seek,
        Follow,
        Attack,
    }
    private void Awake()
    {
        if (fromEntity == null) fromEntity = GetComponent<GameEntity>();
        _gsm = GameEntityStateMachine.CreateStateMachine(transform, fromEntity);
        _gsm.AddRuntimeState(State.Seek.ToString(),OnStartSeek, OnUpdateSeek);
        _gsm.AddRuntimeState(State.Follow.ToString(),null, OnUpdateFollow);
        _gsm.AddRuntimeState(State.Attack.ToString(),OnStartAttack, OnUpdateAttack);
        _gsm.StartAtState(State.Seek.ToString());
    }

    private void Update()
    {
        _gsm.ExecuteStateUpdate();
    }
    
    public void OnStartSeek()
    {
        if (fromEntity.idleAction.GetState() != EntityActionState.InProgress)
        {
            fromEntity.idleAction.StartAction();
        }
    }
    public void OnUpdateSeek()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, seekRadius, layerMask);
        foreach (Collider2D collider in hitColliders)
        {
            var entity = collider.GetComponent<Transform>();
            if (entity)
            {
                chaseTarget = entity;
                var distanceToTarget = Vector3.Distance(chaseTarget.position, fromEntity.transform.position);
                if (distanceToTarget > attackDistance)
                {
                    _gsm.SwitchToState(State.Follow.ToString());
                    return;
                }
            }
        }
    }
    public void OnUpdateFollow()
    {
        if (chaseTarget == null)
        {
            _gsm.SwitchToState(State.Seek.ToString());
            return;
        }

        var distanceToTarget = Vector3.Distance(chaseTarget.position, fromEntity.transform.position);
        if (distanceToTarget > seekRadius)
        {
            chaseTarget = null;
            fromEntity.moveAction.StopAction(EntityActionStopReason.Completed);
            _gsm.SwitchToState(State.Seek.ToString());
            return;
        }

        if (distanceToTarget < attackDistance && fromEntity.moveAction.GetState() == EntityActionState.InProgress)
        {
            fromEntity.moveAction.StopAction(EntityActionStopReason.Completed);
            fromEntity.idleAction.StartAction();
            _gsm.SwitchToState(State.Attack.ToString());
            return;
        }
        
        var moveDirection = chaseTarget.position.x > fromEntity.transform.position.x ? Vector2.right : Vector2.left;
        
        if(fromEntity.moveAction)fromEntity.moveAction.direction = moveDirection;
        if (fromEntity.moveAction.GetState() != EntityActionState.InProgress)
        {
            fromEntity.moveAction.StartAction();
        }
    }

    public void OnStartAttack()
    {
        var direction = chaseTarget.position.x > fromEntity.transform.position.x ? Vector2.right : Vector2.left;
        if(fromEntity.attackAction) fromEntity.attackAction.direction = direction;
        if (fromEntity.attackAction.GetState() != EntityActionState.InProgress)
        {
            fromEntity.attackAction.StartAction();
        }
    }
    public void OnUpdateAttack()
    {
        var distanceToTarget = Vector3.Distance(chaseTarget.position, fromEntity.transform.position);
        if (fromEntity.attackAction.GetState() != EntityActionState.InProgress)
        {
            if (distanceToTarget < attackDistance)
            {
                _gsm.SwitchToState(State.Attack.ToString());
                return;
            }
            else
            {
                chaseTarget = null;
                _gsm.SwitchToState(State.Seek.ToString());
                return;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (_gsm && _gsm.CurrentStateKey == State.Seek.ToString())
        {
            Gizmos.color =  Color.yellow;
            Gizmos.DrawWireSphere(transform.position, seekRadius);
        }
    }
}
