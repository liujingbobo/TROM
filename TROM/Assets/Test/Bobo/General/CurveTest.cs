using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveTest : MonoBehaviour
{
    public AnimationCurve curve;
    public float value = 0;
    private void FixedUpdate()
    {
        print(curve.Evaluate(value));

        value += Time.fixedDeltaTime;
    }
}
