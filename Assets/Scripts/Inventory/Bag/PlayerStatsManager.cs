using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public InventoryData_SO targetBagData;
    public PlayerCharacter playerCharacter;
    public PlayerCombatController playerCombatController;
    
    void Start()
    {
        // playerCharacter = GetComponent<PlayerCharacter>();
        // playerCombatController = GetComponent<PlayerCombatController>();
        //targetBagData = GetComponent<InventoryManager>().targetBagData;
    }

    public void EquipEquipment(ItemData_SO equipment)
    {
        if (equipment == null) return;
        UnEquipEquipment(equipment.itemType);
        Debug.Log("装备装备：" + equipment.itemName);
        InventoryManager.instance.equipmentData.AddEquipment(equipment);
        equipment.isEquipped = true;
        playerCharacter.UpdateCharacterStats();
        if (equipment.itemType == ItemType.Weapon)
        {
            playerCharacter.EquipWeapon(equipment);
            playerCombatController.UpdateNormalComboList(equipment.comboList);
        }
    }

    public void UnEquipEquipment(ItemType itemType)
    {
        InventoryManager.instance.equipmentData.RemoveEquipment(itemType);
        foreach (var oldEquipment in targetBagData.items)
        {
            if(oldEquipment.itemData == null) continue;
            if (oldEquipment.itemData.itemType == itemType && oldEquipment.itemData.isEquipped)
            {
                oldEquipment.itemData.isEquipped = false;
            }
        }
        playerCharacter.UpdateCharacterStats();
        if (itemType == ItemType.Weapon)
        {
            playerCharacter.UnEquipWeapon();
            playerCombatController.UpdateNormalComboList(null);
        }
    }
}