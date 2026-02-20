using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Inventory/EquipmentData")]
public class EquipmentData_SO : ScriptableObject
{
    // 基础属性加成
    public int healthBonus;
    public int manaBonus;
    public int damageBonus;
    public int defenseBonus;
    public float moveSpeedBonus;
    
}
