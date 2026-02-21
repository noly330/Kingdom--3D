using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//包含所有格子
public class ContainerUI : MonoBehaviour
{
    [Header("无需拖拽")]
    public InventoryData_SO bagData;
    public InventoryData_SO targetBag;
    //public GameObject bagSlotHolderPrefab;
    [Header("监听")]
    public ItemTypeEventSO itemTypeWeaponEventSO;
    public ItemTypeEventSO itemTypeFoodEventSO;
    public ItemTypeEventSO itemTypeGobletEventSO;
    public SlotHolder[] slotHolders;

    void Awake()
    {
        if (InventoryManager.instance == null)
        {
            Debug.LogError("InventoryManager单例未初始化", this);
            return;
        }
        // 直接从单例获取数据
        bagData = InventoryManager.instance.inventoryData;
        targetBag = InventoryManager.instance.targetBagData;
    }

    void OnEnable()
    {
        itemTypeWeaponEventSO.OnRaiseEvent += ReFreshUI;
        itemTypeFoodEventSO.OnRaiseEvent += ReFreshUI;
        itemTypeGobletEventSO.OnRaiseEvent += ReFreshUI;

    }
    void OnDisable()
    {
        itemTypeWeaponEventSO.OnRaiseEvent -= ReFreshUI;
        itemTypeFoodEventSO.OnRaiseEvent -= ReFreshUI;
        itemTypeGobletEventSO.OnRaiseEvent -= ReFreshUI;
    }


    public void ReFreshUI(ItemType itemType)
    {
        if (targetBag == null || bagData?.items == null) return;

        UIManager.instance.detailUIManager.detailUI.SetActive(false);
        targetBag.items.Clear();

        for (int i = 0; i < bagData.items.Count; i++)
        {
            var item = bagData.items[i];
            // 添加多层安全检查
            if (item?.itemData?.itemType == itemType)
            {
                targetBag.items.Add(item);
            }
        }

        for (int i = 0; i < slotHolders.Length; i++)
        {
            slotHolders[i].itemUI.Index = i;
            slotHolders[i].UpdateItems();
        }
    }

}
