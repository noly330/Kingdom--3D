using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }
    public PlayerInput playerInput;

    public Vector2 Move => playerInput.actions["Move"].ReadValue<Vector2>();
    public Vector2 Look => playerInput.actions["Look"].ReadValue<Vector2>();
    public Vector2 Scroll => playerInput.actions["Scroll"].ReadValue<Vector2>();
    public bool Run => playerInput.actions["Run"].triggered;
    public bool Jump => playerInput.actions["Jump"].triggered;
    public bool Fire1 => playerInput.actions["Fire1"].triggered;
    public bool Skill1 => playerInput.actions["Skill1"].triggered;
    public bool Skill2 => playerInput.actions["Skill2"].triggered;

    // public bool Slide => playerInput.actions["Slide"].triggered;
    
    private void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        //SceneManager.LoadScene("Playground");
    }

    private void Update()
    {
        if (Fire1)
        {
            Debug.Log("Fire1");
        }
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

