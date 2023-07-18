using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimatorHelper : MonoBehaviour
{
    public bool showDebugLog = false;

    private Dictionary<AnimationStateName, int> _animationHashes = new Dictionary<AnimationStateName, int>();

    public UnityAnimationEvent OnAnimationEventTriggered;

    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        SetUpBasicAnimationEvent();
    }

    public void SetUpBasicAnimationEvent()
    {
        for(int i=0; i<animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "TriggerStartEvent";
            animationStartEvent.stringParameter = clip.name;
            clip.AddEvent(animationStartEvent);
            
            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "TriggerEndEvent";
            animationEndEvent.stringParameter = clip.name;
            clip.AddEvent(animationEndEvent);
        }
    }
    
    public void TriggerStartEvent(string clipName)
    {
        var type = AnimationEventType.AnimationStart;
        if(showDebugLog) Debug.Log($"AnimationStartEventHandler Triggered: clipName:{clipName},type:{type}");
        OnAnimationEventTriggered?.Invoke(type,clipName);
    }
    public void TriggerEndEvent(string clipName)
    {
        var type = AnimationEventType.AnimationEnd;
        if(showDebugLog) Debug.Log($"AnimationEndEventHandler Triggered: clipName:{clipName},type:{type}");
        OnAnimationEventTriggered?.Invoke(type,clipName);
    }
    public void TriggerCustomStringEvent(string dataString)
    {
        var type = AnimationEventType.CustomStringEvent;
        if(showDebugLog) Debug.Log($"AnimationCustomStringEventHandler Triggered: dataString:{dataString},type:{type}");
        OnAnimationEventTriggered?.Invoke(type,dataString);
    }
    
    public void LoadHash(AnimationStateName animationStateName)
    {
        if (!_animationHashes.ContainsKey(animationStateName))
        {
            _animationHashes[animationStateName] = Animator.StringToHash(animationStateName.ToString());
        }
    }
    
    public void Play(AnimationStateName animationStateName, int layer)
    {
        LoadHash(animationStateName);
        animator.Play(_animationHashes[animationStateName],layer);
    }
}

[System.Serializable]
public class UnityAnimationEvent : UnityEvent<AnimationEventType, string>{};

public enum AnimationEventType
{
    AnimationStart,
    AnimationEnd,
    CustomStringEvent,
}

public enum AnimationStateName
{
    SwordShieldMovement,
    SwordShieldNormalAttack,
    ArcherMovement,
    ArcherNormalAttack,
}
