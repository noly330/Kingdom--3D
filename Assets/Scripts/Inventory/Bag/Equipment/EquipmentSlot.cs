using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Image backgroundImage;
    public Image itemImage;

    public void UpdateSlot(ItemData_SO newEquipment)
    {
        if (newEquipment == null)
        {
            backgroundImage.gameObject.SetActive(false);
            return;
        }
        
        
      
        backgroundImage.gameObject.SetActive(true);
        itemImage.sprite = newEquipment.itemIcon;

    }
}
