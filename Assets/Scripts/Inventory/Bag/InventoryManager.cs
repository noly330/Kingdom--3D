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


    [Header("装备管理")]
    public PlayerStatsManager playerStatsManager;
    public PlayerStatsUI playerStatsUI;

    [Header("Containers")]
    public ContainerUI bagContainer;
    public ContainerUI equipmentContainer;
    //public ContainerUI playerStatsContainer;
    [Header("DragCanvas")]
    public Canvas dragCanvas;
    //public DragData currentDrag;

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


    /// <summary>
    /// 暂时先废弃拖拽功能
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>

    #region 废弃:检查拖拽物品是否在每一个slot范围内
    // public bool CheckInInventoryUI(Vector3 position)
    // {
    //     for (int i = 0; i < bagContainer.slotHolders.Length; i++)
    //     {
    //         RectTransform t = bagContainer.slotHolders[i].transform as RectTransform;
    //         if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // public bool CheckInEuqipmentUI(Vector3 position)
    // {
    //     for (int i = 0; i < equipmentContainer.slotHolders.Length; i++)
    //     {
    //         RectTransform t = equipmentContainer.slotHolders[i].transform as RectTransform;
    //         if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // public bool CheckInPlayerStatsUI(Vector3 position)
    // {
    //     for (int i = 0; i < playerStatsContainer.slotHolders.Length; i++)
    //     {
    //         RectTransform t = playerStatsContainer.slotHolders[i].transform as RectTransform;
    //         if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    #endregion
}

        // public class DragData
        // {
        //     public SlotHolder originalHolder;
        //     public RectTransform originalParent;
        // }