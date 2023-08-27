using System;
using System.Collections;
using System.Collections.Generic;
using TimUtility;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIPanel : MonoBehaviour
{
    public StateMachine<PanelState> stateMachine;
    public PanelState panelState;

    protected CanvasGroup CanvasGroup;
    public enum PanelState
    {
        Inactive,
        FadingIn,
        Active,
        FadingOut
    }
    
    public void Initialize()
    {
        stateMachine = new StateMachine<PanelState>();
        stateMachine.AddRuntimeState(PanelState.Inactive, OnInactiveStart, OnInactiveUpdate, OnInactiveExit);
        stateMachine.AddRuntimeState(PanelState.FadingIn, OnFadingInStart, OnFadingInUpdate, OnFadingInExit);
        stateMachine.AddRuntimeState(PanelState.Active, OnActiveStart, OnActiveUpdate, OnActiveExit);
        stateMachine.AddRuntimeState(PanelState.FadingOut, OnFadingOutStart, OnFadingOutUpdate, OnFadingOutExit);
        stateMachine.SwitchToState(PanelState.Inactive);
    }
    public virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }
    
    public virtual void Update()
    {
        stateMachine?.ExecuteStateUpdate();
    }

    public virtual void OpenUI(Action onActiveCallback)
    {
        stateMachine.SwitchToState(PanelState.FadingIn);
        _onActive = onActiveCallback;
    }

    public virtual void CloseUI(Action onInactiveCallback)
    {
        stateMachine.SwitchToState(PanelState.FadingOut);
        _onInactive = onInactiveCallback;
    }

    
    #region Inactive State Actions

    private Action _onInactive;
    public virtual void OnInactiveStart()
    {
        if (CanvasGroup)CanvasGroup.interactable = false;
        gameObject.SetActive(false);
        _onInactive?.Invoke();
    }
    public virtual void OnInactiveUpdate()
    {
        
    }
    public virtual void OnInactiveExit()
    {
        
    }
    #endregion
    
    #region FadingIn State Actions
    protected Countdown fadeInCountdown = new Countdown(0.5f);
    public float fadeInDuration = 0.5f;
    public virtual void OnFadingInStart()
    {
        gameObject.SetActive(true);
        if (CanvasGroup)
        {
            CanvasGroup.interactable = false;
            CanvasGroup.alpha = 0;
            fadeInCountdown.SetCountdownTime(fadeInDuration);
            fadeInCountdown.Flush();
        }
    }
    public virtual void OnFadingInUpdate()
    {
        if (fadeInCountdown.IsCountdownOver())
        {
            stateMachine.SwitchToState(PanelState.Active);
            return;
        }
        if (CanvasGroup)
        {
            CanvasGroup.alpha = Mathf.Lerp(0,1,fadeInCountdown.TimeRatio);
        }
    }
    public virtual void OnFadingInExit()
    {
        if (CanvasGroup)
        {
            CanvasGroup.alpha = 1;
        }
    }
    #endregion
    
    #region Active State Actions
    
    private Action _onActive;
    public virtual void OnActiveStart()
    {
        if (CanvasGroup)CanvasGroup.interactable = true;
        _onActive?.Invoke();
    }
    public virtual void OnActiveUpdate()
    {
        
    }
    public virtual void OnActiveExit()
    {
        
    }
    #endregion
    
    #region FadingOut State Actions
    protected Countdown fadeOutCountdown = new Countdown(0.5f);
    public float fadeOutDuration = 0.5f;
    public virtual void OnFadingOutStart()
    {
        if (CanvasGroup)
        {
            CanvasGroup.interactable = false;
            CanvasGroup.alpha = 1;
            fadeOutCountdown.SetCountdownTime(fadeOutDuration);
            fadeOutCountdown.Flush();
        }
    }
    public virtual void OnFadingOutUpdate()
    {
        if (fadeOutCountdown.IsCountdownOver())
        {
            stateMachine.SwitchToState(PanelState.Inactive);
            return;
        }
        if (CanvasGroup)
        {
            CanvasGroup.alpha = Mathf.Lerp(1,0,fadeOutCountdown.TimeRatio);
        }
    }
    public virtual void OnFadingOutExit()
    {
        if (CanvasGroup)
        {
            CanvasGroup.alpha = 0;
        }
    }
    #endregion
}
