using UnityEngine;

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "ScriptableObjects/ItemConfig", order = 1)]
public class ItemConfigScriptableObjects : ScriptableObject
{
    public ItemID itemId;
    public ItemType itemType;

    public Sprite itemIcon;
    
    public float weightPerItem;
    public int maxStack;
        
}