using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    public float duration = 2f;  //持续时间

    protected virtual void Awake()
    {
        StartCoroutine(IE_DestorySelf());
    }
    IEnumerator IE_DestorySelf()
    {
        while(duration > 0f)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
