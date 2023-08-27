using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_ExploreStartPoint : InteractableObject
{
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
        Debug.Log($"timtest opening map");
        UIStack.Singleton.OpenUIPanel(UIStack.UIPanelTypeName.UIExploreMap);
        return true;
    }

    public override bool TryStopInteract(Interactor2D interactor2D)
    {
        Debug.Log($"timtest closing map");
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
        return true;
    }
}
