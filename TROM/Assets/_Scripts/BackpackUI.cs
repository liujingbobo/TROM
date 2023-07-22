using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    public static BackpackUI Singleton;

    public Button exitButton;
    public Button dropButton;
    public Button equipButton;
    public Button takeButton;
    public Button grabAllButton;
    
    public Transform root;
    public Transform backpackItemSlotParent;
    public GameObject backpackItemSlotPrefab;
    private ChildPrefabAutoList<BackPackItemSlot>  _backpackItemSlotAutoList;
    
    public Transform searchedContainerItemSlotParent;
    public GameObject searchedContainerItemSlotPrefab;
    private ChildPrefabAutoList<BackPackItemSlot> _searchedContainerItemSlotAutoList;
    
    public ItemContainer playerBackpack;
    public ItemContainer searchedContainer;
    public Color selectedColor;
    public Color nonSelectedColor;

    private int _selectedBackPackItemIndex = -1;
    private int _selectedSearchedContainerItemIndex = -1;
    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        _backpackItemSlotAutoList = new ChildPrefabAutoList<BackPackItemSlot>();
        _backpackItemSlotAutoList.Init(backpackItemSlotPrefab, backpackItemSlotParent, BackpackItemSlotSetUpAction);
        
        _searchedContainerItemSlotAutoList = new ChildPrefabAutoList<BackPackItemSlot>();
        _searchedContainerItemSlotAutoList.Init(searchedContainerItemSlotPrefab, searchedContainerItemSlotParent,SearchedContainerItemSlotSetUpAction);
        
        exitButton.onClick.AddListener(CloseUI);
        dropButton.onClick.AddListener(OnClickedDrop);
        equipButton.onClick.AddListener(OnClickedEquip);
        takeButton.onClick.AddListener(OnClickedTake);
        grabAllButton.onClick.AddListener(OnClickedGrabAll);
    }

    private void BackpackItemSlotSetUpAction(BackPackItemSlot slot, int index)
    {
        slot.slotButton.onClick.RemoveAllListeners();
        slot.slotButton.onClick.AddListener(() =>
        {
            if (_selectedBackPackItemIndex == index)
            {
                SetBackPackItemIndex(-1);
            }
            else
            {
                SetBackPackItemIndex(index);
            }
        });
    }
    private void SearchedContainerItemSlotSetUpAction(BackPackItemSlot slot, int index)
    {
        slot.slotButton.onClick.RemoveAllListeners();
        slot.slotButton.onClick.AddListener(() =>
        {
            if (_selectedSearchedContainerItemIndex == index)
            {
                SetSearchedItemIndex(-1);
            }
            else
            {
                SetSearchedItemIndex(index);
            }
        });
    }

    private void SetBackPackItemIndex(int index)
    {
        _selectedBackPackItemIndex = index;
        var list = _backpackItemSlotAutoList.GetDataList();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].slotImage.color = i == _selectedBackPackItemIndex? selectedColor : nonSelectedColor;
        }
    }

    private void SetSearchedItemIndex(int index)
    {
        _selectedSearchedContainerItemIndex = index;
        var list = _searchedContainerItemSlotAutoList.GetDataList();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].slotImage.color = i == _selectedSearchedContainerItemIndex? selectedColor : nonSelectedColor;
        }
    }
    
    private void OnClickedDrop()
    {
        if (_selectedBackPackItemIndex == -1) return;
        if (_selectedBackPackItemIndex >= playerBackpack.itemList.Count) return;
        
        var backpackItem = playerBackpack.itemList[_selectedBackPackItemIndex];
        playerBackpack.RemoveItem(backpackItem);
        searchedContainer.AddItem(backpackItem);
        RefreshItemPanels();
        SetBackPackItemIndex(-1);
        
    }
    
    private void OnClickedEquip()
    {
        if (_selectedBackPackItemIndex == -1) return;
    }
    
    private void OnClickedTake()
    {
        if (_selectedSearchedContainerItemIndex == -1) return;
        if (_selectedSearchedContainerItemIndex >= searchedContainer.itemList.Count) return;
        
        var item = searchedContainer.itemList[_selectedSearchedContainerItemIndex];
        searchedContainer.RemoveItem(item);
        playerBackpack.AddItem(item);
        RefreshItemPanels();
        SetSearchedItemIndex(-1);
    }
    
    private void OnClickedGrabAll()
    {
        var allCount = searchedContainer.itemList.Count;
        for (int i = 0; i < allCount; i++)
        {
            var item = searchedContainer.itemList[0];
            searchedContainer.RemoveItem(item);
            playerBackpack.AddItem(item);
        }
        RefreshItemPanels();
        SetSearchedItemIndex(-1);
    }
    
    private void RefreshItemPanels()
    {
        _backpackItemSlotAutoList.ShowByCount(playerBackpack.itemList.Count);
        for (int i = 0; i < playerBackpack.itemList.Count; i++)
        {
            var item = playerBackpack.itemList[i];
            _backpackItemSlotAutoList.GetChildObjectAndData(i, out var go, out var itemSlot);
            itemSlot.itemIamge.sprite = ItemManager.Singleton.GetItemConfig(item.itemId).itemIcon;
        }
        
        _searchedContainerItemSlotAutoList.ShowByCount(searchedContainer.itemList.Count);
        for (int i = 0; i < searchedContainer.itemList.Count; i++)
        {
            var item = searchedContainer.itemList[i];
            _searchedContainerItemSlotAutoList.GetChildObjectAndData(i, out var go, out var itemSlot);
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

public class BackPackItemSlot : IAutoListChildData
{
    public Image slotImage;
    public Image itemIamge;
    public Button slotButton;
    
    public IAutoListChildData Instantiate()
    {
        return new BackPackItemSlot();
    }

    public void ReadDataFromGameObject(GameObject go)
    {
        slotImage = go.GetComponent<Image>();
        itemIamge = go.transform.GetChild(0).GetComponent<Image>();
        slotButton = go.transform.GetComponent<Button>();
    }
}
