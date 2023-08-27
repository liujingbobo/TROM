using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TIMTEST : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            UIStack.Singleton.OpenUIPanel(UIStack.UIPanelTypeName.UIPanel);
        }
    }
}
