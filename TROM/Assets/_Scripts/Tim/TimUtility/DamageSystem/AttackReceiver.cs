using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackReceiver : MonoBehaviour
{
    public GameTeam team;
    public Collider receiverCollider;
    public AttackReceiverGetAttackEvent OnGetAttacked;

    public void GetAttack(AttackReleaseInfo info)
    {
        Debug.Log($"Get Attacked");
        OnGetAttacked?.Invoke(info);
    }
    
    
    [System.Serializable]
    public class AttackReceiverGetAttackEvent : UnityEvent<AttackReleaseInfo>{};
}
