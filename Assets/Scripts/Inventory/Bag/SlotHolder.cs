using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotHolder : MonoBehaviour
{
    public ItemUI itemUI;
    [Header("广播")]
    public ItemDataEventSO itemDataEventSO;
    [Header("无需拖拽")]
    public InventoryData_SO targetBag;
    public ItemData_SO currentItemData;

    public void UpdateItems()
    {
        if(targetBag == null)
            targetBag = InventoryManager.instance.targetBagData;

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
