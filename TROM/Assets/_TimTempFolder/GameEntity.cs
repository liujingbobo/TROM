using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rBody2D;

    public EntityIdle idle;
    public EntitySimpleMove moveAction;
    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rBody2D == null) rBody2D = GetComponent<Rigidbody2D>();
    }

    [Button]
    private void StartMoveAction()
    {
        moveAction.StartAction();
    }
}
