using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player
{    
    private void Awake_BindInput()
    {
        GameObject obj = GameObject.Find("InputSystem");
        PlayerInput playerInput = obj.GetComponent<PlayerInput>();
        InputActionMap map = playerInput.actions.FindActionMap("Player");

        //Move
        {
            InputAction action = map.FindAction("Move");
            action.performed += (context) =>
            {
                inputMove = context.ReadValue<Vector2>();
            };
            action.canceled += (context) => inputMove = Vector2.zero;
        }

        //Attack
        {
            InputAction action = map.FindAction("Attack");
            action.performed += (context) =>
            {
                OnAttack?.Invoke();
            };
        }
    }

    
}
