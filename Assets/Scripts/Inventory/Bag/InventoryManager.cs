using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [Header("背包数据")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO equipmentData;
    public InventoryData_SO targetBagData;


    [Header("装备")]
    public PlayerStatsManager playerStatsManager;

    [Header("Containers")]
    public ContainerUI bagContainer;
    public ContainerUI equipmentContainer;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        bagContainer.ReFreshUI(ItemType.Weapon);
        equipmentContainer.ReFreshUI(ItemType.Weapon);

        //游戏一开始就加载装备
        foreach (var i in equipmentData.items)
        {
            if(i.itemData == null) continue;
            Debug.Log("自动加载装备：" + i.itemData.itemName);
            playerStatsManager.EquipEquipment(i.itemData);
        }
    }
}