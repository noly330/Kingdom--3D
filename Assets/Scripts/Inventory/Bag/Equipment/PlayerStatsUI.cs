using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text maxHealthText;
    public Text damageText;
    public Text defenseText;

    public void UpdateStatsUI(float currentMaxHealth,float currentAttack)
    {
        maxHealthText.text = "最大生命：" + currentMaxHealth.ToString("F0");
        damageText.text = "攻击力：" + currentAttack.ToString("F0");
    }
}
