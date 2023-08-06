using PlayerControllerTest;
using UnityEngine;

[RequireComponent(typeof(ItemContainer))]
public class IO_SceneItemContainer : InteractableObject
{
    [SerializeField] private ItemContainer targetContainer;
    
    public void Start()
    {
        interactIcon.gameObject.SetActive(false);
    }

    public override bool CheckRegistrable(Interactor2D interactor2D)
    {
        return true;
    }


    public override bool TryInteract(Interactor2D interactor2D, params object[] objects)
    {
        if (interactor2D.GetComponentInChildren<FSM>() is { } sm)
        {
            if (sm.CurrentState == PlayerState.Idle)
            {
                sm.OpenBackPack(targetContainer);
                return true;
            }
        }
        return false;
    }

    public override bool TryStopInteract(Interactor2D interactor2D)
    {
        if (targetContainer.IsEmpty())
        {
            // TODO: this is just temp.
            Destroy(this);
        }

        return true;
    }

    public override void UpdateState(params object[] objects)
    {
    }

    public override void OnRegistered(Interactor2D interactor2D)
    {
        base.OnRegistered(interactor2D);
        interactIcon.gameObject.SetActive(true);
    }

    public override void OnUnRegistered(Interactor2D interactor2D)
    {
        base.OnUnRegistered(interactor2D);
        interactIcon.gameObject.SetActive(false);
    }
    
    public override bool CheckInteractable(Interactor2D interactor2D)
    {
        if (interactor2D.GetComponentInChildren<FSM>() is { } sm)
        {
        }
        return true;
    }
}
