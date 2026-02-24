using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("无需拖拽方便观察")]
    public PlayerStatsUI playerStatsUI;
    public DetailUIManager detailUIManager;
    public InteractPrompt interactPrompt;
    public FadeManager fadeManager;
    public PlayerInfoUI playerInfoUI;
    public DialogueUI dialogueUI;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        playerStatsUI = GetComponent<PlayerStatsUI>();
        detailUIManager = GetComponent<DetailUIManager>();
        interactPrompt = GetComponent<InteractPrompt>();
        fadeManager = GetComponent<FadeManager>();
        playerInfoUI = GetComponent<PlayerInfoUI>();
        dialogueUI = GetComponent<DialogueUI>();

    }
}
