using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PlayerControls playerControls;

    public new static InputManager Instance => Singleton<InputManager>.Instance;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement() 
        => playerControls.Player.Movement.ReadValue<Vector2>();
    
    public Vector2 GetPlayerLookMouseDelta() 
        => playerControls.Player.Look.ReadValue<Vector2>();

    public bool GetPlayerSprintButtonDown()
        => playerControls.Player.Sprint.triggered;


    public bool DebugKeyPressed(Key key) 
        => Keyboard.current[key].wasPressedThisFrame;
    public bool DebugKeyHeld(Key key) 
        => Keyboard.current[key].isPressed;
}
