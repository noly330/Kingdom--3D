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
    public UIState currentUIState = UIState.None;
    public GameObject bagPanel;
    public GameObject equipPanel;
    public GameObject settingPanel;
    public GameObject playerInfoPanel;

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
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnClickSetting();
        }

    }


    public void OnClickBag()
    {
        if(currentUIState!= UIState.None && currentUIState!= UIState.Bag)
            return;
        if(bagPanel.activeSelf)
        {
            currentUIState = UIState.None;
            bagPanel.SetActive(false);
            playerInput.actions.FindActionMap("Player").Enable();

            playerInfoPanel.SetActive(true);
        }
        else
        {
            currentUIState = UIState.Bag;
            bagPanel.SetActive(true);
            UIManager.instance.interactPrompt.HidePrompt();
            playerInput.actions.FindActionMap("Player").Disable();

            playerInfoPanel.SetActive(false);
        }
    }
    public void OnClickEquip()
    {
        if(currentUIState!= UIState.None && currentUIState!= UIState.Equip)
            return;
        if(equipPanel.activeSelf)
        {
            currentUIState = UIState.None;
            equipPanel.SetActive(false);
            playerInput.actions.FindActionMap("Player").Enable();

            playerInfoPanel.SetActive(true);
        }
        else
        {
            currentUIState = UIState.Equip;
            equipPanel.SetActive(true);
            UIManager.instance.interactPrompt.HidePrompt();
            playerInput.actions.FindActionMap("Player").Disable();

            playerInfoPanel.SetActive(false);
        }
    }
    public void OnClickSetting()
    {
        if(currentUIState!= UIState.None && currentUIState!= UIState.Setting)
            return;
        if(settingPanel.activeSelf)
        {
            currentUIState = UIState.None;
            settingPanel.SetActive(false);
            playerInput.actions.FindActionMap("Player").Enable();

            playerInfoPanel.SetActive(true);
        }
        else
        {
            currentUIState = UIState.Setting;
            settingPanel.SetActive(true);
            UIManager.instance.interactPrompt.HidePrompt();
            playerInput.actions.FindActionMap("Player").Disable();

            playerInfoPanel.SetActive(false);
        }
    }
}

public enum UIState
{
    None,
    Bag,
    Equip,
    Setting,
}
