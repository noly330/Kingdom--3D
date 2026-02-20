using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerControl playerControl;
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

            playerControl = new PlayerControl();
    }

    void OnEnable()
    {
        playerControl.Enable();
    }

    void OnDisable()
    {
        playerControl.Disable();
    }
}
