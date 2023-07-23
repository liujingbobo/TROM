using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SceneItemContainer : ItemContainer
{
    public SpriteRenderer interactIcon;
    public void Start()
    {
        interactIcon.gameObject.SetActive(false);
    }

    public void OnInteractionInput(Interactor2D interactor2D, InteractableInput input)
    {
        if (input == InteractableInput.InteractionStart)
        {
            BackpackUI.Singleton.OpenUI(this);
        }
    }
    public void OnInteractableNearestStateChanged(Interactor2D interactor2D, bool isNearest)
    {
        interactIcon.gameObject.SetActive(isNearest);
    }

    [Button]
    public void TestAddRandomItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var randomNum = UnityEngine.Random.Range(1, Enum.GetValues(typeof(ItemID)).Length);
            var randID = (ItemID) randomNum;
            var newItem = (new GameObject(randID.ToString())).AddComponent<Item>();
            newItem.Init(randID, 1);
            AddItem(newItem);
        }
    }
}
