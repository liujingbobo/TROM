using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class A_Parallax : MonoBehaviour
{
    public Camera cam;

    public Vector3 OriPos;
    public bool Set = false;
    public Vector2 effector;

    public CinemachineVirtualCamera camm;
    public CinemachineCore core;

    private void Awake()
    {
        Set = false;
    }

    private void LateUpdate()
    {
        if (!Set)
        {
            OriPos = camm.transform.position;
            Set = true;
        }
        else
        {
            Vector3 curPos = camm.transform.position;
            Vector3 offSet = curPos - OriPos;
            transform.position = new Vector3(offSet.x * effector.x, offSet.y * effector.y, transform.position.z);
        }
    }
}
