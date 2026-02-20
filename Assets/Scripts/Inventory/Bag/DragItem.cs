// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;

// [RequireComponent(typeof(ItemUI))]
// public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// {
//     ItemUI currentItemUI;
//     SlotHolder currentHolder;
//     SlotHolder targetHolder;
    
//     void Awake()
//     {
//         currentItemUI = GetComponent<ItemUI>();
//         currentHolder = GetComponentInParent<SlotHolder>();
//     }
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         InventoryManager.instance.currentDrag = new InventoryManager.DragData();
//         InventoryManager.instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>(); 
//         InventoryManager.instance.currentDrag.originalParent = (RectTransform)transform.parent;
//         transform.SetParent(InventoryManager.instance.dragCanvas.transform,true);
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         transform.position = eventData.position;
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         //是否指向ui物品
//         if (EventSystem.current.IsPointerOverGameObject())
//         {
//             if(InventoryManager.instance.CheckInInventoryUI(eventData.position) 
//             || InventoryManager.instance.CheckInEuqipmentUI(eventData.position) 
//             || InventoryManager.instance.CheckInPlayerStatsUI(eventData.position))
//             {
//                 if(eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
//                     targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
//                 else
//                     targetHolder = eventData.pointerEnter.GetComponentInParent<SlotHolder>();
//                 Debug.Log(eventData.pointerEnter.gameObject);
//                 switch (targetHolder.slotType)
//                 {
//                     case SlotType.Bag:
//                         SwapItem();
//                         break;
//                     case SlotType.Equipment:
                        
//                         break;
//                 }
//                 currentHolder.UpdateItems();
//                 targetHolder.UpdateItems();
//             }
//         }
//         transform.SetParent(InventoryManager.instance.currentDrag.originalParent,true);
//     }

//     public void SwapItem()
//     {
//         var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
//         var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

//         bool isSameItem = tempItem.itemData == targetItem.itemData;
//         if(isSameItem && targetItem.itemData.isStackable )
//         {
//             targetItem.amount += tempItem.amount;
//             tempItem.itemData = null;
//             tempItem.amount = 0;
//         }
//         else
//         {
//             currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
//             targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
//         }
//     }
// }
