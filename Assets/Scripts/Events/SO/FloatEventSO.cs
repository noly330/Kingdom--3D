using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu( fileName = "New Float Event", menuName = "Events/Float Event")]
public class FloatEventSO : ScriptableObject
{
    public UnityAction<float> OnRaiseEvent;
    public void RaiseEvent(float value)
    {
        OnRaiseEvent.Invoke(value);
    }
}
