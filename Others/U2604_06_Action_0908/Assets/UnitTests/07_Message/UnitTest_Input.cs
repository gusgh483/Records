using UnityEngine;
using UnityEngine.InputSystem;

public class UnitTest_Input : MonoBehaviour
{
    public void OnMove(InputValue value)
    {
        print($"Key : {value.Get<Vector2>()}");
    }
}
