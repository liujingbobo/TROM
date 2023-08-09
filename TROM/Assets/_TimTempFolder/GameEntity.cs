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

    private void Update()
    {
    }

    #region FaceDirection

    public FacingDirection facingDirection = FacingDirection.Right;
    public bool IsFacingRight
    {
        get => facingDirection == FacingDirection.Right;
    }
    public void SetFacingDirection(FacingDirection direction)
    {
        facingDirection = direction;
        spriteRenderer.flipX = !IsFacingRight;
    }

    [Button]
    public void LookAt(FacingDirection direction)
    {
        SetFacingDirection(direction);
    }

    #endregion
    
    public void GetAttacked(AttackReleaseInfo info)
    {
        if (info.team == GameTeam.Player)
        {
            controller.GetStaggered();
        }
    }
}
