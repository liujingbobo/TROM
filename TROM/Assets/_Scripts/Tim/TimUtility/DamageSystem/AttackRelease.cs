using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackRelease : MonoBehaviour
{
    public AttackReleaseInfo attackReleaseInfo;
    public bool triggerCallbackEnabled = false;

    public Dictionary<AttackReceiver, float> TriggeredReceivers = new Dictionary<AttackReceiver, float>();
    public void Init(AttackReleaseInfo info)
    {
        attackReleaseInfo = info;
        Invoke("DeleteSelf", info.duration);
        triggerCallbackEnabled = true;
        TriggeredReceivers.Clear();
    }

    private void DeleteSelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCallbackEnabled) return;
        var receiver = other.GetComponent<AttackReceiver>();
        if (receiver != null)
        {
            if (TriggeredReceivers.ContainsKey(receiver))
            {
                return;
            }
            TriggeredReceivers.Add(receiver, Time.time);
            
            Debug.Log($"timtest Triggered Attack to {receiver.transform.parent.name}");
            receiver.GetAttack(attackReleaseInfo);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggerCallbackEnabled) return;
        var receiver = other.GetComponent<AttackReceiver>();
        if (receiver != null)
        {
            if (TriggeredReceivers.ContainsKey(receiver))
            {
                return;
            }

            if (receiver.team == attackReleaseInfo.team) return;
            
            TriggeredReceivers.Add(receiver, Time.time);
            
            Debug.Log($"timtest Triggered Attack to {receiver.transform.parent.name}");
            receiver.GetAttack(attackReleaseInfo);
        }
    }
}

public enum GameTeam
{
    Player,
    Enemy,
}
public class AttackReleaseInfo
{
    public string objectName = "attackRelease";
    public GameTeam team = GameTeam.Player;
    public GameEntity fromEntity;
    public Vector3 worldPos;
    public Vector3 localScale;
    public float duration;
    public float damage;
}

public static class AttackReleaseExtension
{
    public static AttackRelease CreateAttackRelease2D(AttackReleaseInfo info)
    {
        var attackReleaseOBJ = new GameObject($"{info.objectName}");
        attackReleaseOBJ.transform.position = info.worldPos;
        attackReleaseOBJ.transform.localScale = info.localScale;

        var attackRelease = attackReleaseOBJ.AddComponent<AttackRelease>();

        //add trigger
        var boxCollider = attackReleaseOBJ.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        boxCollider.size = Vector2.one;

        //add rigid body
        var rb2D = attackReleaseOBJ.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;
        
        attackRelease.Init(info);
        return attackRelease;
    }
}