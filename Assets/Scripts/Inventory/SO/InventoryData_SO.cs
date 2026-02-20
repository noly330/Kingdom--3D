using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "SO/Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();
    //public List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot>();

    public void AddItem(ItemData_SO newItem, int amount = 1)
    {
        bool found = false;
        if (newItem.isStackable)
        {
            foreach (var item in items)
            {
                if (item.itemData == newItem)
                {
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == null && !found)
            {
                items[i].itemData = newItem;
                items[i].amount = amount;
                break;
            }
        }
    }

    #region 装备相关
    public void AddEquipment(ItemData_SO newEquipment)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].itemData == null)
            {
                items[i].itemData = newEquipment;
                items[i].amount = 1;
                break;
            }
        }
    }

    public void RemoveEquipment(ItemType equipmentType)
    {
        
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].itemData == null) continue;
            if (items[i].itemData.itemType == equipmentType)
            {
                items[i].itemData = null;
            }
        }
    }
    #endregion
}


[System.Serializable]
public class InventoryItem
{
    public ItemData_SO itemData;

    public int amount;
}

// [System.Serializable]
// public class EquipmentSlot
// {
//     public ItemType slotType;
//     public InventoryItem equippedItem;
//     public bool isEmpty => equippedItem == null;
// }
