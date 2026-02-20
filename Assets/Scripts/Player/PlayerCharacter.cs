using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    public Transform weaponSlot;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        UpdateCharacterStats();
    }


    #region 武器相关
    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon == null) return;
        Instantiate(weapon.weaponPrefab, weaponSlot);
    }
    public void UnEquipWeapon()
    {
        foreach (Transform child in weaponSlot)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    public void UpdateCharacterStats()
    {
        currentAttack = baseAttack;
        InventoryData_SO equipmentData = InventoryManager.instance.equipmentData;
        foreach (var i in equipmentData.items)
        {
            if(i.itemData == null) continue;
            currentAttack += i.itemData.equipmentData.damageBonus;
        }

    }

}
