using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    private Dictionary<ItemID, ItemConfigScriptableObjects> _itemIdToConfigDictionary =
        new Dictionary<ItemID, ItemConfigScriptableObjects>();
    
    private void Start()
    {
        LoadItemConfigs();
    }

    private void LoadItemConfigs()
    {
        _itemIdToConfigDictionary.Clear();
        foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
        {
            var address = $"Assets/_Scripts/ItemConfigs/{id}.asset";
            var op = Addressables.LoadAssetAsync<ItemConfigScriptableObjects>(address);
            ItemConfigScriptableObjects so = op.WaitForCompletion();
            _itemIdToConfigDictionary.Add(id, so);
        }
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
    Wood,
    Parts,
    
    CannedFood,
    Apple,
    
    BottledWater,
    BowlOfWater,
    
}

public enum ItemType
{
    Material,
    Consumable,
}