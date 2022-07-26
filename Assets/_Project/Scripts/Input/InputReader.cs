using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 movetest;

    public Vector2 move;
    public Vector2 mouseScreenPos;

    private Controls _controls;
    private void Awake()
    {
        _controls = new Controls();
        _controls.Player.SetCallbacks(this);
        
        _controls.Player.Enable();
    }

    private void OnDestroy()
    {
        _controls.Player.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    public void OnFreelook(InputAction.CallbackContext context)
    {
        movetest = context.ReadValue<Vector2>();
    }
}
