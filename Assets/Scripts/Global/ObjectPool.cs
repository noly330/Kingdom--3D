using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<Pool> pools;
    private Dictionary<ObjectPoolType, Queue<GameObject>> poolDictionary;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<ObjectPoolType, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);

            }
            poolDictionary.Add(pool.poolType, poolQueue);
        }
    }

    public GameObject SpawnFromPool(ObjectPoolType poolType, Vector3 position, Quaternion rotation)
    {
 
        if (!poolDictionary.ContainsKey(poolType))
        {
            Debug.LogError("没有这个对象池" + poolType);
            return null;
        }

        GameObject obj = poolDictionary[poolType].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    public void ReturnPool(ObjectPoolType poolType, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[poolType].Enqueue(obj);

    }
}



[System.Serializable]
public class Pool
{
    public GameObject prefab;
    public ObjectPoolType poolType;
    public int size;
}