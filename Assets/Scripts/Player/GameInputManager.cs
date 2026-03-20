using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }
    public PlayerInput playerInput;

    public Vector2 Move => playerInput.actions["Move"].ReadValue<Vector2>();
    public Vector2 Look => playerInput.actions["Look"].ReadValue<Vector2>();
    public Vector2 Scroll => playerInput.actions["Scroll"].ReadValue<Vector2>();
    public bool Run => playerInput.actions["Run"].triggered;

    public bool Fire1 => playerInput.actions["Fire1"].triggered;
    // public bool Slide => playerInput.actions["Slide"].triggered;
    
    private void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerInput.actions.Enable();
    }
    void OnDisable()
    {
        playerInput.actions.Disable();
    }
}

