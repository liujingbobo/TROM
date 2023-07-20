using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    public Camera cam;

    public Vector3 PrePosition;
    public bool Set = false;
    public Vector2 effector;

    private void Awake()
    {
        Set = false;
    }

    private void LateUpdate()
    {
        if (!Set)
        {
            PrePosition = cam.transform.position;
            Set = true;
        }
        else
        {
            Vector3 curPos = cam.transform.position;
            Vector3 offSet = curPos - PrePosition;
            var transform1 = transform;
            transform1.position = new Vector3(offSet.x * effector.x, offSet.y * effector.y, transform1.position.z);
        }
    }
}
