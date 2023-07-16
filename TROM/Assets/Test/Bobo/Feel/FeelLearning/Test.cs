using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

/// <summary>
/// A very simple class used to make a character jump, designed to be used in Feel's Getting Started tutorial
/// </summary>
public class GettingStartedTutorialHero : MonoBehaviour
{
    [Header("Hero Settings")]
    /// a key the Player has to press to make our Hero jump
    public KeyCode ActionKey = KeyCode.Space;
    /// the force to apply vertically to the Hero's rigidbody to make it jump up
    public float JumpForce = 8f;

    [Header("Feedbacks")]
    /// a MMFeedbacks to play when the Hero starts jumping
    public MMF_Player JumpFeedback;
    /// a MMFeedbacks to play when the Hero lands after a jump
    public MMF_Player LandingFeedback;

    private const float _lowVelocity = 0.1f;
    private Rigidbody2D _rigidbody;
    private float _velocityLastFrame;
    private bool _jumping = false;
    public float maxVelocity;

    /// <summary>
    /// On Awake we store our Rigidbody and force gravity to -30 on the y axis so that jumps feel better
    /// </summary>
    private void Awake()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        Physics2D.gravity = Vector2.down * 30;
        maxVelocity = 0;
    }

    public float time;

    /// <summary>
    /// Every frame
    /// </summary>
    private void Update()
    {
        // we check if the Player has pressed our action key, and trigger a jump if that's the case
        if (Input.GetKeyDown(ActionKey) && !_jumping)
        {
            Jump();
        }
        else if (_jumping)
        {
            time += Time.deltaTime;
        }
        
        // if we're jumping, were going down last frame, and have now reached an almost null velocity
        if (_jumping && (_velocityLastFrame < 0) && (Mathf.Abs(_rigidbody.velocity.y) < _lowVelocity))
        {
            // then we just landed, we reset our state
            _jumping = false;
            if(LandingFeedback) LandingFeedback.PlayFeedbacks();
        }
        
        // we store our velocity
        _velocityLastFrame = _rigidbody.velocity.y;

        if (_velocityLastFrame > maxVelocity) maxVelocity = _velocityLastFrame;
    }

    /// <summary>
    /// Makes our hero jump in the air
    /// </summary>
    private void Jump()
    {
        time = 0;
        
        if(JumpFeedback) JumpFeedback.PlayFeedbacks();
        _rigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        _jumping = true;
    }
}
