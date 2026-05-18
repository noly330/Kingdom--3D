using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu( fileName = "ItemTypeEvent", menuName = "Events/ItemTypeEvent")]
public class ItemTypeEventSO : ScriptableObject
{
    public UnityAction<ItemType> OnRaiseEvent;
    public ItemType itemType;

    public void RaiseEvent()
    {
        OnRaiseEvent?.Invoke(itemType);
    }
}
