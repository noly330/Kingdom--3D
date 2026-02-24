using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentDialogueData;

    private GameObject player;
    bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && player.GetComponent<PlayerInput>().actions["Interact"].triggered)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        UIManager.instance.dialogueUI.UpdateDialogueData(currentDialogueData);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentDialogueData != null)
        {
            player = other.gameObject;
            isPlayerInRange = true;
            UIManager.instance.interactPrompt.ShowPrompt(InteractType.Dialogue);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            isPlayerInRange = false;
            UIManager.instance.interactPrompt.HidePrompt();
        }
    }
}
