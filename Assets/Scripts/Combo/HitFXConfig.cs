using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="HitFXConfig",menuName ="SO/Combat/HitFXConfig")]
public class HitFXConfig : ScriptableObject
{
    [SerializeField] private GameObject[] hitFXList;

    public GameObject TryGetOneFXObj()
    {
        return hitFXList[Random.Range(0,hitFXList.Length)] ;
    }
}
