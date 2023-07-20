using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] public bool grounded;

    public CustomColliderDetector frontHungDetector;
    public CustomColliderDetector backHungDetector;
}
