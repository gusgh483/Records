using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UnitTest_Input3 : MonoBehaviour
{
    [SerializeField]
    private UnityEvent unityEvent;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            print("Unity Event Called");

            unityEvent.Invoke();
        }
    }

    public void OnCharacterMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Debug.Log($"UnityEvent value: {value}");
    }
}
