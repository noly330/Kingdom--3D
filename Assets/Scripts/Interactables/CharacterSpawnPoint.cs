using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterSpawnPoint : MonoBehaviour
{
    public Transform[] spawnPoints;
    public ObjectPoolType CharacterPoolType;
    private bool isPlayerInRange = false;
    private GameObject player;

    private void Update()
    {
        if (isPlayerInRange && GameInputManager.Instance.Interact)
        {
            SpawnCharacter();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject;
            UIManager.instance.interactPrompt.ShowPrompt(InteractType.Use);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
            UIManager.instance.interactPrompt.HidePrompt();
        }
    }

    private void SpawnCharacter()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject character = ObjectPool.instance.SpawnFromPool(CharacterPoolType, spawnPoints[i].position, spawnPoints[i].rotation);

            //Nav的bug，不得不对敌人做特殊处理了，以后自己写寻路
            NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPoints[i].position);
            }
        }
    }
}
