using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RayCastExtensions
{
    public static bool CameraMainRaycastFirstHitPos(Vector2 screenPos, out Vector3 worldPos, int layerMask = -1)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity, layerMask))
        {
            worldPos = hit.point;
            return true;
        }
        worldPos = Vector3.zero;
        return false;
    }
}
