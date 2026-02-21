using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    private bool isPlayerInRange = false;
    private GameObject player;

    void Update()
    {
        if (isPlayerInRange && player.GetComponent<PlayerInput>().actions["Pick Up"].triggered)
        {
            OnPickUpItem();
        }
    }
    public void OnPickUpItem()
    {

        InventoryManager playerBag = player.GetComponent<InventoryManager>();
        playerBag.inventoryData.AddItem(itemData, itemData.itemAmount);
        playerBag.bagContainer.ReFreshUI(itemData.itemType);
        UIManager.instance.interactPrompt.HidePrompt();
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject;
            UIManager.instance.interactPrompt.ShowPrompt(InteractType.PickUp);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
            UIManager.instance.interactPrompt.HidePrompt();
        }
    }
}