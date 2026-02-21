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

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

        playerStatsUI=GetComponent<PlayerStatsUI>();
        detailUIManager=GetComponent<DetailUIManager>();
        interactPrompt=GetComponent<InteractPrompt>();

    }
}
