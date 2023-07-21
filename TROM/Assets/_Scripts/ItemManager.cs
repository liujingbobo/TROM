using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Singleton;
    
    private Dictionary<ItemID, ItemConfigScriptableObjects> _itemIdToConfigDictionary =
        new Dictionary<ItemID, ItemConfigScriptableObjects>();

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        LoadItemConfigs();
    }

    private void LoadItemConfigs()
    {
        _itemIdToConfigDictionary.Clear();
        foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
        {
            if(id == ItemID.Null) continue;
            var address = $"Assets/_Scripts/ItemConfigs/{id}.asset";
            var op = Addressables.LoadAssetAsync<ItemConfigScriptableObjects>(address);
            ItemConfigScriptableObjects so = op.WaitForCompletion();
            _itemIdToConfigDictionary.Add(id, so);
        }
    }

    public ItemConfigScriptableObjects GetItemConfig(ItemID itemID)
    {
        return _itemIdToConfigDictionary[itemID];
    }

    [Button]
    public void LogConfigs()
    {
        foreach (var kvp in _itemIdToConfigDictionary)
        {
            Debug.Log($"{kvp.Key}");
        }
    }

    [Button]
    public void DropItem(ItemID itemId)
    {
        var newGO = new GameObject(itemId.ToString());
        newGO.transform.position = transform.position;

        var rb2d = newGO.AddComponent<Rigidbody2D>();
        var circleCollider = newGO.AddComponent<CircleCollider2D>();
        var sr = newGO.AddComponent<SpriteRenderer>();
        sr.sprite = _itemIdToConfigDictionary[itemId].itemIcon;
    }
}

public enum ItemID
{
    Null,
    Wood,
    Parts,
    
    CannedFood,
    Apple,
    
    BottledWater,
    BowlOfWater,
    
}

public enum ItemType
{
    Null,
    Material,
    Consumable,
}