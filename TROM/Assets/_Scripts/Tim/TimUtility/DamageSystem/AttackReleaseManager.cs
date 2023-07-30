using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AttackReleaseManager : MonoBehaviour
{
    public Transform targetTransform;

    [Button]
    public void CreateTestHitBoxOnTargetTransform()
    {
        var attackInfo = new AttackReleaseInfo()
        {
            objectName = "attackRelease",
            fromEntity = null,
            worldPos = targetTransform.position,
            localScale = Vector3.one,
            duration = 1,
            damage = 1,
        };
        AttackReleaseExtension.CreateAttackRelease2D(attackInfo);
    }
}
