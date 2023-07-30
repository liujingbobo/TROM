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
    public MonsterController controller;
    
    public virtual void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animatorHelper == null) animatorHelper = GetComponent<AnimatorHelper>();
        if (rBody2D == null) rBody2D = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GetAttacked(AttackReleaseInfo info)
    {
        controller.GetStaggered();
    }
}
