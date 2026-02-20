using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Item Data", menuName = "SO/Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;

    [TextArea]
    public string itemDescription = "";
    public bool isStackable;

    [Header("装备信息")]
    public bool isEquipped;
    //武器需要生成在游戏中所以需要预制体，其他装备拿数据就行了
    public GameObject weaponPrefab;
    public EquipmentData_SO equipmentData;
    public ComboListSO comboList;

    [Header("食物信息")]
    public float hpGain;
}
