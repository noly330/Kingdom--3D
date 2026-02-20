using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemIcon = null;
    public Text showAmount = null;
    public int Index { get; set; } = -1;

    public void SetupItemUI(ItemData_SO itemData, int amount = 1)
    {
        if (itemData == null)
        {
            itemIcon.gameObject.SetActive(false);
            showAmount.gameObject.SetActive(false);
            return;
        }


        itemIcon.sprite = itemData.itemIcon;
        this.showAmount.text = amount.ToString();

        itemIcon.gameObject.SetActive(true);
        this.showAmount.gameObject.SetActive(true);

    }

}
