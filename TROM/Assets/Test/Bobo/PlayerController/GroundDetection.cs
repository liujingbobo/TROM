using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private PlayerDetection Detection;
    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     var temp = 1 << col.gameObject.layer;
    //     if (temp !=groundLayer) return;
    //     
    //     Detection.grounded = true;
    //     print("Touch Ground!!");
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     var temp = 1 << other.gameObject.layer;
    //     if (temp !=groundLayer) return;
    //
    //     Detection.grounded = false;
    //     print("Leave Ground!!");
    // }
}
