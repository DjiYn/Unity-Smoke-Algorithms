using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.PlayerActions onFoot;
    public PlayerInput.PlayerActions OnFoot { get { return onFoot; } set { onFoot = value; } }


    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.Player;
    }

    private void Update()
    {
    }

    private void OnEnable() 
    { 
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
