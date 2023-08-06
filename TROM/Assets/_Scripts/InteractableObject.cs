using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public SpriteRenderer interactIcon;
    
    public Interactor2D interactingInteractor;

    public abstract bool TryInteract(Interactor2D interactor2D, params object[] objects);

    public abstract bool TryStopInteract(Interactor2D interactor2D);

    public abstract void UpdateState(params object[] objects);
    
    // This only indicate interactor enter, might have a slight highlight for interactableobject
    // This will show that this is a interactableObject, but doesn't mean that it can be interacted. 
    public virtual void OnInteractorEnter(Interactor2D interactor2D)
    {
    }

    public virtual void OnInteractorExit(Interactor2D interactor2D)
    {
    }

    // There can be only one IO registered at the same time, "Nearest". 
    public virtual void OnRegistered(Interactor2D interactor2D)
    {
        interactIcon.gameObject.SetActive(true);
    }
    
    public virtual void OnUnRegistered(Interactor2D interactor2D)
    {
        interactIcon.gameObject.SetActive(false);
    }
    
    public virtual bool CheckInteractable(Interactor2D interactor2D)
    {
        return false;
    }
    
    public virtual bool CheckRegistrable(Interactor2D interactor2D)
    {
        return false;
    }
}