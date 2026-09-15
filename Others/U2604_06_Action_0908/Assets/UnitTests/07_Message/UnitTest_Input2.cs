using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitTest_Input2 : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveAction;

    private void Awake()
    {
        Action<InputAction.CallbackContext> callback = (context) =>
        {
            Vector2 value = context.ReadValue<Vector2>();
            Debug.Log($"InputActionReference value: {value}");
        };

        moveAction.action.performed += callback;
        moveAction.action.canceled += callback;
    }

    private void OnCharacterMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log($"InputActionReference value: {value}");
    }

    private void Update()
    {
        //Vector2 value = moveAction.action.ReadValue<Vector2>();
        //Debug.Log($"InputActionReference value: {value}");
    }
}
