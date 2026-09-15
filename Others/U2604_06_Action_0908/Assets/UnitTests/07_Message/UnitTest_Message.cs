using UnityEngine;

public class UnitTest_Message : MonoBehaviour
{
    private GameObject UnitTest_A;

    private void Awake()
    {
        UnitTest_A = GameObject.Find("UnitTest_A");
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UnitTest_A.SendMessage("Recieve_Message", "Send Message");
            UnitTest_A.BroadcastMessage("Recieve_Message", "Broadcast Message");
        }
    }
}
