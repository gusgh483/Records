using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitTest_Input4 : MonoBehaviour
{
    private void Awake()
    {
        //RAII
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("Player");

        //Move
        {
            Action<InputAction.CallbackContext> callback = (context) =>
            {
                Vector2 value = context.ReadValue<Vector2>();
                Debug.Log($"InputActionReference value: {value}");
            };

            InputAction action = actionMap["Move"];
            action.performed += callback;
            action.canceled += callback;
        }
    }
}
