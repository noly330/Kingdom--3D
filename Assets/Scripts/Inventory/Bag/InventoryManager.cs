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

    [Header("监听")]
    [SerializeField] private VoidEventSO onLoadEvent;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {
        onLoadEvent.OnRaiseEvent += LoadEquipmentData;
    }
    void OnDisable()
    {
        onLoadEvent.OnRaiseEvent -= LoadEquipmentData;
    }

    void Start()
    {
        bagContainer.ReFreshUI(ItemType.Weapon);
        equipmentContainer.ReFreshUI(ItemType.Weapon);

        //游戏一开始就加载装备，写好存档功能以后记得删除
        LoadEquipmentData();
    }

    void LoadEquipmentData()
    {
        bool hasWeapon = false;
        bool hasGoblet = false;
        foreach (var i in equipmentData.items)
        {
            if(i.itemData == null) continue;
            Debug.Log("自动加载装备：" + i.itemData.itemName);
            if(i.itemData.itemType == ItemType.Weapon)
                hasWeapon = true;
            else if(i.itemData.itemType == ItemType.Goblet)
                hasGoblet = true;
            
            playerStatsManager.EquipEquipment(i.itemData);
        }

        //自动卸下装备
        if(!hasWeapon)
            playerStatsManager.UnEquipEquipment(ItemType.Weapon);
        if(!hasGoblet)
            playerStatsManager.UnEquipEquipment(ItemType.Goblet);
    }
}