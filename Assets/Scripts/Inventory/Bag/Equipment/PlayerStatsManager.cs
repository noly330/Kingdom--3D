using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public InventoryData_SO targetBagData;
    public PlayerCharacter playerCharacter;


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

        //更新UI
        playerCharacter.UpdateCharacterStats();
        UIManager.instance.playerStatsUI.UpdateEquipmentUI();
        UIManager.instance.playerStatsUI.UpdateStatsUI(playerCharacter);

        if (equipment.itemType == ItemType.Weapon)
        {
            playerCharacter.EquipWeapon(equipment);
            //playerCombatController.UpdateNormalComboList(equipment.comboList);
        }
    }

    public void UnEquipEquipment(ItemType itemType)
    {
        InventoryManager.instance.equipmentData.RemoveEquipment(itemType);
        foreach (var oldEquipment in targetBagData.items)
        {
            if (oldEquipment.itemData == null) continue;
            if (oldEquipment.itemData.itemType == itemType && oldEquipment.itemData.isEquipped)
            {
                oldEquipment.itemData.isEquipped = false;
            }
        }

        //更新UI
        playerCharacter.UpdateCharacterStats();
        UIManager.instance.playerStatsUI.UpdateEquipmentUI();
        UIManager.instance.playerStatsUI.UpdateStatsUI(playerCharacter);

        if (itemType == ItemType.Weapon)
        {
            playerCharacter.UnEquipWeapon();
            //playerCombatController.UpdateNormalComboList(null);
        }
    }

    public void UsedFood(ItemData_SO food)
    {
        if (food == null) return;
        Debug.Log("使用食物：" + food.itemName);
        InventoryManager.instance.inventoryData.RemoveItem(food, 1, false);
        InventoryManager.instance.bagContainer.ReFreshUI(food.itemType);
        playerCharacter.currentHealth += food.hpGain;
        playerCharacter.currentHealth = Mathf.Clamp(playerCharacter.currentHealth, 0, playerCharacter.maxHealth);
    }
}