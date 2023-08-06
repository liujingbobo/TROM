using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// TODO: maybe later switch this to "NOT MONOBEHAVIOUR", act as value
public class ItemContainer : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();
    public float maxWeight;
    public float maxSlot; // TODO:
    public float CurrentWeight => itemList.Sum(x => x.weight);
    
    public void AddItem(Item item)
    {
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        itemList.Add(item);
    }
    public bool RemoveItem(Item item)
    {
        if (!itemList.Contains(item)) throw new Exception("Given Item Not This Container");
        
        var succes = itemList.Remove(item);
        return succes;
    }
    public bool IsEmpty()
    {
        return CurrentWeight == 0;
    }
    public bool IsFull()
    {
        return CurrentWeight <= maxWeight;
    }
    public bool CanAddItem(Item item)
    {
        return CurrentWeight + item.weight <= maxWeight;
    }
    
    [Button]
    public void TestAddItem(ItemID id)
    {
        var newItem = (new GameObject(id.ToString())).AddComponent<Item>();
        newItem.Init(id, 1);
        AddItem(newItem);
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
