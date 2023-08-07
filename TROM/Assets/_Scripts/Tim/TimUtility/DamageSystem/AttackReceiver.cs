using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackReceiver : MonoBehaviour
{
    public GameTeam team;
    public AttackReceiverGetAttackEvent OnGetAttacked;

    public void GetAttack(AttackReleaseInfo info)
    {
        if (info.team != team)
        {
            OnGetAttacked?.Invoke(info);
        }
    }
    
    
    [System.Serializable]
    public class AttackReceiverGetAttackEvent : UnityEvent<AttackReleaseInfo>{};
}
