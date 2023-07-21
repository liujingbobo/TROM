using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable2D : MonoBehaviour
{
    public Collider2D triggerCollider;
    public LayerMask triggerLayer;
    public List<Interactor2D> registeredInteractors = new List<Interactor2D>();

    public InteractionInputEvent interactionInputEvent;
    public InteractableAsNearestEvent interactionAsNearestEvent;
    
    public virtual void InteractionStart(Interactor2D interactor2D)
    {
        interactionInputEvent?.Invoke(interactor2D, InteractableInput.InteractionStart);
    }
    public virtual void InteractionHold(Interactor2D interactor2D)
    {
        interactionInputEvent?.Invoke(interactor2D, InteractableInput.InteractionHold);
    }
    public virtual void InteractionEnd(Interactor2D interactor2D)
    {
        interactionInputEvent?.Invoke(interactor2D, InteractableInput.InteractionEnd);
    }
    
    public void RegisterToInteractor(Interactor2D interactor2D)
    {
        registeredInteractors.Add(interactor2D);
        interactor2D.RegisterInteractable(this);
    }

    public void OnSetAsNearestByInteractor(Interactor2D interactor2D)
    {
        interactionAsNearestEvent?.Invoke(interactor2D, true);
    }

    public void OnUnsetNearestByInteractor(Interactor2D interactor2D)
    {
        interactionAsNearestEvent?.Invoke(interactor2D, false);
    }
    
    public void UnregisterFromInteractor(Interactor2D interactor2D)
    {
        if (registeredInteractors.Contains(interactor2D))
        {
            registeredInteractors.Remove(interactor2D);
            interactor2D.UnregisterInteractable(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((triggerLayer & (1 << other.gameObject.layer)) != 0 )
        {
            var interactor = other.gameObject.GetComponent<Interactor2D>();
            if (interactor)
            {
                RegisterToInteractor(interactor);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((triggerLayer & (1 << other.gameObject.layer)) != 0 )
        {
            var interactor = other.gameObject.GetComponent<Interactor2D>();
            if (interactor)
            {
                UnregisterFromInteractor(interactor);
            }
        }
    }
}

public enum InteractableInput
{
    InteractionStart,
    InteractionHold,
    InteractionEnd,
}
[System.Serializable]
public class InteractionInputEvent : UnityEvent<Interactor2D, InteractableInput> { }
[System.Serializable]
public class InteractableAsNearestEvent : UnityEvent<Interactor2D, bool> { }