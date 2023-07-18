using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public Animator animator;
    public AnimatorHelper animatorHelper;
    public Rigidbody2D rBody2D;
    public SpriteRenderer spriteRenderer;

    public EntityIdle idleAction;
    public EntitySimpleMove moveAction;
    public EntitySimpleAttack attackAction;
    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animatorHelper == null) animatorHelper = GetComponent<AnimatorHelper>();
        if (rBody2D == null) rBody2D = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [Button]
    private void MoveRight()
    {
        moveAction.direction = Vector2.right;
        moveAction.StartAction();
    }
    [Button]
    private void MoveLeft()
    {
        moveAction.direction = Vector2.left;
        moveAction.StartAction();
    }
    
    [Button]
    private void Attack()
    {
        attackAction.StartAction();
    }
}
