using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSpawnPoint : MonoBehaviour
{
    public Transform[] spawnPoints;
    public ObjectPoolType CharacterPoolType;
    private bool isPlayerInRange = false;
    private GameObject player;

    private void Update()
    {
        if (isPlayerInRange && player.GetComponent<PlayerInput>().actions["Interact"].triggered)
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
        }
    }
}
