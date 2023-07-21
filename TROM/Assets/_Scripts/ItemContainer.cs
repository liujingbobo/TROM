using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    public float currentWeight;
    public float maxWeight;
    public float maxSlot;
    
    public void Start()
    {
    }
    
    public void AddItem(Item item)
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        
        itemList.Add(item);
        currentWeight = itemList.Sum(x => x.weight);
    }

    [Button]
    public void TestAddItem(ItemID id)
    {
        var newItem = (new GameObject(id.ToString())).AddComponent<Item>();
        newItem.Init(id, 1);
        AddItem(newItem);
    }
}
