using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    public static BackpackUI Singleton;

    public Button exitButton;
    
    public Transform root;
    public Transform backpackItemSlotParent;
    public GameObject backpackItemSlotPrefab;
    public AutoChildPrefabGrower backpackItemSlotGrower;
    
    public Transform searchedContainerItemSlotParent;
    public GameObject searchedContainerItemSlotPrefab;
    public AutoChildPrefabGrower searchedContainerItemSlotGrower;
    
    public ItemContainer playerBackpack;
    public ItemContainer searchedContainer;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        backpackItemSlotGrower.Init<ItemSlot>(backpackItemSlotPrefab, backpackItemSlotParent);
        searchedContainerItemSlotGrower.Init<ItemSlot>(searchedContainerItemSlotPrefab, searchedContainerItemSlotParent);
        
        exitButton.onClick.AddListener(CloseUI);
    }

    private void RefreshItemPanels()
    {
        backpackItemSlotGrower.ShowByCount<ItemSlot>(playerBackpack.itemList.Count);
        for (int i = 0; i < playerBackpack.itemList.Count; i++)
        {
            var item = playerBackpack.itemList[i];
            backpackItemSlotGrower.GetChildObjectAndData<ItemSlot>(i, out var go, out var itemSlot);
            itemSlot.itemIamge.sprite = ItemManager.Singleton.GetItemConfig(item.itemId).itemIcon;
        }
        
        searchedContainerItemSlotGrower.ShowByCount<ItemSlot>(searchedContainer.itemList.Count);
        for (int i = 0; i < searchedContainer.itemList.Count; i++)
        {
            var item = searchedContainer.itemList[i];
            searchedContainerItemSlotGrower.GetChildObjectAndData<ItemSlot>(i, out var go, out var itemSlot);
            itemSlot.itemIamge.sprite = ItemManager.Singleton.GetItemConfig(item.itemId).itemIcon;
        }
    }
    
    public void OpenUI(ItemContainer searchedContainer)
    {
        root.gameObject.SetActive(true);
        this.searchedContainer = searchedContainer;
        RefreshItemPanels();
    }

    public void CloseUI()
    {
        searchedContainer = null;
        root.gameObject.SetActive(false);
    }

    [Button]
    public void TestOpenUI()
    {
        OpenUI(searchedContainer);
    }
}

public class ItemSlot : IAutoGrowChildData
{
    public Image slotImage;
    public Image itemIamge;
    
    public IAutoGrowChildData Instantiate()
    {
        return new ItemSlot();
    }

    public void ReadDataFromGameObject(GameObject go)
    {
        slotImage = go.GetComponent<Image>();
        itemIamge = go.transform.GetChild(0).GetComponent<Image>();
    }
}
