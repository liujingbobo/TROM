using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class GameEntity : MonoBehaviour, IHasHealthBar
{
    public AnimatorHelper animatorHelper;
    public Rigidbody2D rBody2D;
    public MonsterController controller;
    public SkeletonAnimation SkeletonAnimation;
    
    public virtual void Awake()
    {
        if (animatorHelper == null) animatorHelper = GetComponent<AnimatorHelper>();
        if (rBody2D == null) rBody2D = GetComponent<Rigidbody2D>();

        MaxHealth = 10;
        CurrentHealth = 10;
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
        var localScale = SkeletonAnimation.transform.localScale;
        SkeletonAnimation.transform.localScale = localScale.SetX( (IsFacingRight ? 1f : -1f) * Mathf.Abs(localScale.x));
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
            CurrentHealth = Mathf.Clamp( CurrentHealth - (int) info.damage, 0 , MaxHealth);
        }
    }

    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
}
