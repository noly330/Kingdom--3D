using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    // public static UIManager instance;
    public PlayerInput playerInput;

    [Header("界面")]
    public GameObject bagPanel;
    public GameObject equipPanel;
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
        }
        else
        {
            bagPanel.SetActive(true);
            isBagOpen = true;
            playerInput.actions.FindActionMap("Player").Disable();
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
        }
        else
        {
            equipPanel.SetActive(true);
            isEquipOpen = true;
            playerInput.actions.FindActionMap("Player").Disable();
        }
    }
    


}
