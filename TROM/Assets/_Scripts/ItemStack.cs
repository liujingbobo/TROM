using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    public SpriteRenderer interactIcon;


    private void Start()
    {
        interactIcon.gameObject.SetActive(false);
    }

    public void OnInteractionInput(Interactor2D interactor2D, InteractableInput input)
    {
        //trigger main usage here, await player interaction call
        if (input == InteractableInput.InteractionStart)
        {
            Debug.Log("opening stack ui");
        }
    }
    public void OnInteractableNearestStateChanged(Interactor2D interactor2D, bool isNearest)
    {
        interactIcon.gameObject.SetActive(isNearest);
    }
    
}
