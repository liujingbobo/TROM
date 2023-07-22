using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemID itemId;
    public ItemType itemType;

    public float weight;
    public float durability;


    public void Init(ItemID itemID, float inputDurability)
    {
        this.itemId = itemID;
        var config = ItemManager.Singleton.GetItemConfig(itemID);
        this.itemType = config.itemType;
        weight = config.weightPerItem;
        durability = inputDurability;
    }
}
