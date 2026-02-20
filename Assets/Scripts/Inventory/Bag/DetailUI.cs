using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailUI : MonoBehaviour
{
    [Header("物品详细基本信息")]
    public GameObject detailUI;
    public GameObject confirmButton;
    public Text confirmButtonText;
    public Image itemImage;
    public Text itemTypeText;
    public Text itemNameText;
    public Text itemDescripText;
    
    [Header("监听")]
    public ItemDataEventSO itemDataDetailEventSO;

    private ItemData_SO currentItemData;


    void Awake()
    {
    }

    void OnEnable()
    {
        itemDataDetailEventSO.OnRaiseEvent += ShowDetail;
    }
    void OnDisable()
    {
        itemDataDetailEventSO.OnRaiseEvent -= ShowDetail;
    }

    public GameObject bagPanel;
    public GameObject equipmentPanel;
    
    void Update()
    {
        //如果两个面板都关闭了，就关闭自己
        if (!bagPanel.activeSelf && !equipmentPanel.activeSelf)
        {
            detailUI.SetActive(false);
        }
    }

    private void ShowDetail(ItemData_SO itemdata)
    {
        if(itemdata == null)
        {
            detailUI.SetActive(false);
            return;
        }
        
        currentItemData = itemdata;
        detailUI.SetActive(true);
        itemImage.sprite = itemdata.itemIcon;
        itemTypeText.text = itemdata.itemType.ToString();
        itemNameText.text = itemdata.itemName;
        itemDescripText.text = itemdata.itemDescription;

        switch (itemdata.itemType)
        {
            case ItemType.Weapon:
                confirmButton.SetActive(true);
                if(itemdata.isEquipped)
                    confirmButtonText.text = "取消装备";
                else
                    confirmButtonText.text = "装备";
                break;

            case ItemType.Goblet:
                confirmButton.SetActive(true);
                if(itemdata.isEquipped)
                    confirmButtonText.text = "取消装备";
                else
                    confirmButtonText.text = "装备";
                break;
            case ItemType.Food:
                confirmButton.SetActive(true);
                confirmButtonText.text = "使用";
                break;

            default:
                confirmButton.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 点击确认按钮的时候
    /// </summary>
    public void OnClickConfirmButton()
    {
        if(currentItemData.itemType == ItemType.Weapon || currentItemData.itemType == ItemType.Goblet)
        {
            UpdateEquipment();
            return;
        }
        else if(currentItemData.itemType == ItemType.Food)
        {
            OnUsedFood();
            return;
        }
        
    }

    private void UpdateEquipment()
    {
        if(currentItemData.isEquipped)
        {
            //取消装备
            InventoryManager.instance.playerStatsManager.UnEquipEquipment(currentItemData.itemType);
            currentItemData.isEquipped = false;
        }
        else
        {
            //装备
            InventoryManager.instance.playerStatsManager.EquipEquipment(currentItemData);
            currentItemData.isEquipped = true;
        }
        ShowDetail(currentItemData);
    }

    private void OnUsedFood()
    {
        InventoryManager.instance.playerStatsManager.UsedFood(currentItemData);
        detailUI.SetActive(false);
    }
    
}
