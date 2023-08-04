using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.Utilities;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private PlayerDetection Detection;

    private List<Collider2D> collider2Ds = new List<Collider2D>();
}
