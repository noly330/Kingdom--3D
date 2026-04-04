using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 暂时没用，后续再考虑是否需要
/// </summary>
public abstract class PoolItemBase : MonoBehaviour, IPoolItem
{

    private void OnEnable()
    {
        Spawn();
    }

    private void OnDisable() 
    {
        Recycle();
    }
    public virtual void Spawn()
    {

    }
    public virtual void Recycle()
    {

    }

}

public interface IPoolItem
{
    void Spawn();
    void Recycle();
}
