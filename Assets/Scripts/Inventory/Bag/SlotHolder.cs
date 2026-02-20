using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHolder : MonoBehaviour
{
    public SlotType slotType;
    public ItemUI itemUI;
    [Header("广播")]
    public ItemDataEventSO itemDataEventSO;
    [Header("无需拖拽")]
    public InventoryData_SO targetBag;
    public ItemData_SO currentItemData;

    // public void  UpdateItems(ItemType itemType)
    // {
    //     // switch (slotType)
    //     // {
    //     //     case SlotType.Bag:
    //     //         itemUI.Bag = bagData;
    //     //         break;
    //     //     case SlotType.Equipment:

    //     //         break;
    //     // }

    //     // var item = itemUI.Bag.items[itemUI.Index];
    //     // itemUI.SetupItemUI(item.itemData,item.amount);
    //     if(bagData.items[itemUI.Index].itemData.itemType == itemType)
    //     {
    //         var item = bagData.items[itemUI.Index];
    //         itemUI.SetupItemUI(item.itemData,item.amount);
    //     }

    // }
    void Awake()
    {
        if (InventoryManager.instance == null)
        {
            Debug.LogError("InventoryManager单例未初始化！", this);
            return;
        }
        targetBag = InventoryManager.instance.targetBagData;
    }

    public void UpdateItems()
    {
        if(targetBag == null)
            targetBag = InventoryManager.instance.targetBagData;
        switch (slotType)
        {
            case SlotType.Bag:
                itemUI.Bag = targetBag;
                break;
            case SlotType.Equipment:

                break;
        }
        // 检查当前物品
        if(itemUI.Index >= targetBag.items.Count)
        {
            itemUI.SetupItemUI(null, 0);
            return;
        }
        var currentItem = targetBag.items[itemUI.Index];
        if (currentItem == null)
        {
            itemUI.SetupItemUI(null, 0);
            return;
        }

        // 检查物品数据
        if (currentItem.itemData == null)
        {
            itemUI.SetupItemUI(null, 0);
            return;
        }
        currentItemData = currentItem.itemData;

        itemUI.SetupItemUI(currentItem.itemData, currentItem.amount);
        
    }

    public void OnClickItemSlot()
    {
        if(currentItemData == null)
            return;
        itemDataEventSO.RaiseEvent(currentItemData);
    }


}
