using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public static class EventCenter
{
    private static Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();


    //where T : class：
    // 泛型约束：T必须是引用类型
    // protobuf消息都是class，所以这个约束很合理
    public static void Addlistener<T>(Action<T> listener) where T : class
    {
        if(listener == null)  throw new ArgumentNullException(nameof(listener));
        var type = typeof(T);
        if(_events.TryGetValue(type,out var existingDelegate))  //尝试从字典获取已有委托
        {
            //combine就是+=
            _events[type] = Delegate.Combine(existingDelegate,listener);
        }
        else
        {
            _events[type] = listener;
        }
    }

    public static void RemoveListener<T>(Action<T> listener) where T : class
    {
        if(listener == null)  throw new ArgumentNullException(nameof(listener));
        var type = typeof(T);
        if(_events.TryGetValue(type,out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate,listener);
            if(newDelegate == null)
            {
                _events.Remove(type);
            }
            else
            {
                _events[type] = newDelegate;
            }

        }
    }

    public static void Broadcast<T>(T message)where T : class
    {
        if(message == null)  throw new ArgumentNullException(nameof(message));
        if(_events.TryGetValue(typeof(T),out var delegateToInvoke))
        {
            (delegateToInvoke as Action<T>)?.Invoke(message);
        }
    }
}
