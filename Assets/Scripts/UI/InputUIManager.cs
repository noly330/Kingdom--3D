using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUIManager : MonoBehaviour
{
    // public static UIManager instance;
    public PlayerInput playerInput;

    [Header("界面")]
    public GameObject bagPanel;
    public GameObject equipPanel;
    public GameObject playerInfoPanel;
    private bool isBagOpen = false;
    private bool isEquipOpen = false;

    void Awake()
    {

    }

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            OnClickBag();
        }
        if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            OnClickEquip();
        }
    }

    public void OnClickBag()
    {
        if(isEquipOpen)  return;
        if(bagPanel.activeSelf)
        {
            bagPanel.SetActive(false);
            isBagOpen = false;
            playerInput.actions.FindActionMap("Player").Enable();

            playerInfoPanel.SetActive(true);
        }
        else
        {
            bagPanel.SetActive(true);
            UIManager.instance.interactPrompt.HidePrompt();
            isBagOpen = true;
            playerInput.actions.FindActionMap("Player").Disable();

            playerInfoPanel.SetActive(false);
        }
    }
    public void OnClickEquip()
    {
        if(isBagOpen)  return;
        if(equipPanel.activeSelf)
        {
            equipPanel.SetActive(false);
            isEquipOpen = false;
            playerInput.actions.FindActionMap("Player").Enable();

            playerInfoPanel.SetActive(true);
        }
        else
        {
            equipPanel.SetActive(true);
            UIManager.instance.interactPrompt.HidePrompt();
            isEquipOpen = true;
            playerInput.actions.FindActionMap("Player").Disable();

            playerInfoPanel.SetActive(false);
        }
    }
}
