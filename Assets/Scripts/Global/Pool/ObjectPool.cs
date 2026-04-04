using System.Collections.Generic;
using UnityEngine;
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    [SerializeField] private List<Pool> _pools;
    private Dictionary<ObjectPoolType, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        InitPool();
    }

    private void InitPool()
    {
        _poolDictionary = new Dictionary<ObjectPoolType, Queue<GameObject>>();
        foreach (Pool pool in _pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();
            GameObject poolItemParent = new GameObject(pool.poolType.ToString());
            poolItemParent.transform.SetParent(transform);

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, poolItemParent.transform);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);

            }
            _poolDictionary.Add(pool.poolType, poolQueue);
        }
    }

    public GameObject SpawnFromPool(ObjectPoolType poolType, Vector3 position, Quaternion rotation)
    {
 
        if (!_poolDictionary.ContainsKey(poolType))
        {
            Debug.LogError("对象池中没有这类物品：" + poolType);
            return null;
        }

        GameObject obj = _poolDictionary[poolType].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    public void ReturnPool(ObjectPoolType poolType, GameObject obj)
    {
        if (!_poolDictionary.ContainsKey(poolType))
        {
            Debug.LogError("对象池中没有这类物品：" + poolType);
            return;
        }
        obj.SetActive(false);
        _poolDictionary[poolType].Enqueue(obj);

    }
}

[System.Serializable]
public class Pool
{
    public GameObject prefab;
    public ObjectPoolType poolType;
    public int size;
}