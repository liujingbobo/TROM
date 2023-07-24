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

    public readonly CustomColliderDetector hangDetector;

    public PlayerDetection(CustomColliderDetector hangDetector)
    {
        this.hangDetector = hangDetector;
    }
}
