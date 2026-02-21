using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    public Transform weaponSlot;
    private InventoryData_SO equipmentData;
    public VoidEventSO HealthUpdayeEvent;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        UpdateCharacterStats();
    }

    void OnEnable()
    {

    }
    void OnDisable()
    {

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
        float healthPercentage = currentHealth / maxHealth;

        currentAttack = baseAttack;
        maxHealth = baseHealth;
        equipmentData = InventoryManager.instance.equipmentData;
        foreach (var i in equipmentData.items)
        {
            if (i.itemData == null) continue;
            currentAttack += i.itemData.equipmentData.damageBonus;
            maxHealth += i.itemData.equipmentData.healthBonus;
        }

        UIManager.instance.playerStatsUI.UpdateStatsUI(maxHealth, currentAttack);

        currentHealth = maxHealth * healthPercentage;
        // 确保不溢出
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    }

}
