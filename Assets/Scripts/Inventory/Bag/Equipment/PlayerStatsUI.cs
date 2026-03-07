using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public EquipmentSlot weaponSlot;
    public EquipmentSlot gobletSlot;
    private InventoryData_SO equipmentData;
    public Text maxHealthText;
    public Text damageText;
    public Text critChange;
    public Text defenseText;

    public void UpdateEquipmentUI()
    {
        equipmentData = InventoryManager.instance.equipmentData;

        ItemData_SO weapon = null;
        ItemData_SO goblet = null;

        foreach(var i in equipmentData.items)
        {
            if(i.itemData == null) 
                continue;  //防止访问空物品报错

            if(i.itemData.itemType == ItemType.Weapon)
                weapon = i.itemData;
            else if(i.itemData.itemType == ItemType.Goblet)
                goblet = i.itemData;
        }
        weaponSlot.UpdateSlot(weapon);
        gobletSlot.UpdateSlot(goblet);
    }


    public void UpdateStatsUI(CharacterBase characterBase)
    {
        maxHealthText.text = "最大生命：" + characterBase.maxHealth.ToString("F0");
        damageText.text = "攻击力：" + characterBase.currentAttack.ToString("F0");
        critChange.text = "暴击率：" + (characterBase.currentCritChance * 100).ToString("F0") + "%";
    }
}
