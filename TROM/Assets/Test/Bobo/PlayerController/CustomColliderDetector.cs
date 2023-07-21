using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class CustomColliderDetector : MonoBehaviour
{
    public LayerMask targetLayer;

    public bool checkCustomLayer;

    public List<CustomLayer> targetCustomLayer;

    public List<Collider2D> collider2Ds = new List<Collider2D>();
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (targetLayer.MMContains(col.gameObject.layer))
        {
            if (checkCustomLayer)
            {
                if (col.GetComponent<GeneralCollider>() is { } gc)
                {
                    if (targetCustomLayer.Contains(gc.CTLayer))
                    {
                        collider2Ds.Add(col);
                    }
                }
            }
            else
            {
                collider2Ds.Add(col);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (targetLayer.MMContains(other.gameObject.layer))
        {
            if (checkCustomLayer)
            {
                if (other.GetComponent<GeneralCollider>() is { } gc)
                {
                    if (targetCustomLayer.Contains(gc.CTLayer))
                    {
                        if (collider2Ds.Contains(other))
                        {
                            collider2Ds.Remove(other);
                        }
                    }
                }
            }
            else
            {
                if (collider2Ds.Contains(other))
                {
                    collider2Ds.Remove(other);
                }
            }
        }
    }
    public enum CustomLayer
    {
        Ground,
        Hung
    }
}
