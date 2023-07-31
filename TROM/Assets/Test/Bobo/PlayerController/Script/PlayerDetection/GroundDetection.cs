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
    
    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     var targetLayer = 1 << col.gameObject.layer;
    //     if (targetLayer !=groundLayer) return;
    //
    //     collider2Ds.Add(col);
    //     SetGrounded();
    //     print("Touch Ground!!");
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     var targetLayer = 1 << other.gameObject.layer;
    //     if (targetLayer !=groundLayer) return;
    //
    //     if (collider2Ds.Contains(other)) collider2Ds.Remove(other);
    //     
    //     SetGrounded();
    //     print("Leave Ground!!");
    // }
    //
    // void SetGrounded()
    // {
    //     Detection.grounded = !collider2Ds.IsNullOrEmpty();
    // }
}
