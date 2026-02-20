using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public EquipmentSlot weaponSlot;
    public EquipmentSlot gobletSlot;
    private InventoryData_SO equipmentData;

    public void UpdateEquipmentUI()
    {
        equipmentData = InventoryManager.instance.equipmentData;

        ItemData_SO weapon = null;
        ItemData_SO goblet = null;

        foreach(var i in equipmentData.items)
        {
            if(i.itemData == null) 
                continue;
            if(i.itemData.itemType == ItemType.Weapon)
                weapon = i.itemData;
            else if(i.itemData.itemType == ItemType.Goblet)
                goblet = i.itemData;
        }
        weaponSlot.UpdateSlot(weapon);
        gobletSlot.UpdateSlot(goblet);
    }
}
