using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{

    [System.Serializable]
    public class LootItem
    {
        public GameObject item;
        [Range(0, 1)]
        public float weight;
    }

    public LootItem[] lootItems;

    public void CreatLootItem()
    {
        for (int i = 0; i < lootItems.Length; i++)
        {
            float currentValue = Random.value;
            if (currentValue < lootItems[i].weight)
            {
                GameObject obj = Instantiate(lootItems[i].item, transform.position, transform.rotation);
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 randomDir = Random.insideUnitCircle.normalized;
                    Vector3 force = new Vector3(randomDir.x, 1f, randomDir.z) * 3f;
                    rb.AddForce(force, ForceMode.Impulse);
                }

            }
        }
    }

}
