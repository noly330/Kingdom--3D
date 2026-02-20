using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu( fileName = "ItemDataEvent", menuName = "Events/ItemDataEvent")]
public class ItemDataEventSO : ScriptableObject
{
   public UnityAction<ItemData_SO> OnRaiseEvent;

   public void RaiseEvent(ItemData_SO itemData)
   {
       OnRaiseEvent?.Invoke(itemData);
   }
}
