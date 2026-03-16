using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour
{
    public void PlayFootSound()
    {
        ObjectPool.instance.SpawnFromPool(ObjectPoolType.FootEffect, transform.position, transform.rotation);
    }
}
