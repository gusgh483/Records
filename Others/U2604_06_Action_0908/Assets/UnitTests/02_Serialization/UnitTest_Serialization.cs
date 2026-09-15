using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public string Name;
    public float HP;
    public float Attack;
    public float Defence;
}

[Serializable]
public enum WeaponType
{
    Sword, Axe, Hammer, Bow, Wand, Max,
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct WeaponData
{
    public WeaponType Type;
    public float Power;
    public float Speed;
    public float Durability;
    public float CriticalRatio;
}

public class UnitTest_Serialization : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    public PlayerData PlayerData => playerData;

    [SerializeField]
    private string fileName;
}

[CustomEditor(typeof(UnitTest_Serialization))]
public class Serialization_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        //Debug.Log("OnInspectorGUI");

        serializedObject.Update();

        SerializedProperty playerDataProp = serializedObject.FindProperty("playerData");

        SerializedProperty nameProp = playerDataProp.FindPropertyRelative("Name");
        EditorGUILayout.PropertyField(nameProp, new GUIContent("Name"));

        SerializedProperty hpProp = playerDataProp.FindPropertyRelative("HP");
        EditorGUILayout.PropertyField(hpProp, new GUIContent("HP"));

        SerializedProperty attackProp = playerDataProp.FindPropertyRelative("Attack");
        EditorGUILayout.PropertyField(attackProp, new GUIContent("Attack"));

        SerializedProperty defenceProp = playerDataProp.FindPropertyRelative("Defence");
        EditorGUILayout.PropertyField(defenceProp, new GUIContent("Defence"));

        serializedObject.ApplyModifiedProperties();


        UnitTest_Serialization serial = (UnitTest_Serialization)target;
        string path = "Assets/UnitTests/02_Serialization/";


        if (GUILayout.Button("1_Save Json"))
        {
            //Debug.Log(nameProp.stringValue);
            //Debug.Log(hpProp.floatValue);
            //Debug.Log(attackProp.floatValue);
            //Debug.Log(defenceProp.floatValue);

            UnitTest_File.SaveJson(path + "1_Json.txt", serial.PlayerData);
        }

        if (GUILayout.Button("2_Save Json"))
        {
            UnitTest_File.SaveJson2(path + "2_Json.txt", serial.PlayerData);
        }

        if (GUILayout.Button("3_Oepn Json"))
        {
            UnitTest_File.OpenJson(path + "1_Json.txt");
            //UnitTest_File.OpenJson(path + "2_Json.txt");
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("4_Save Binary"))
        {
            UnitTest_File.SaveBinaryFile(path + "3_Binary.data", serial.PlayerData);
        }

        if (GUILayout.Button("5_Read Binary"))
        {
            UnitTest_File.OpenBinaryFile(path + "3_Binary.data");
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("6_Save_WeaponData"))
        {
            int count = UnityEngine.Random.Range(5, 10 + 1);

            WeaponData[] datas = new WeaponData[count];
            for (int i = 0; i < count; i++)
            {
                WeaponData data = new WeaponData();

                int type = UnityEngine.Random.Range(0, (int)WeaponType.Max);
                data.Type = (WeaponType)type;

                data.Power = UnityEngine.Random.Range(5.0f, 20.0f);
                data.Speed = UnityEngine.Random.Range(0.5f, 2.0f);
                data.Durability = UnityEngine.Random.Range(1.0f, 3.0f);
                data.CriticalRatio = UnityEngine.Random.Range(0.1f, 0.25f);

                datas[i] = data;
            }

            Debug.Log($"{count}개의 WeaponData 생성");


            UnitTest_File.SaveMashal(path + "4_Binary.data", datas);
        }

        if (GUILayout.Button("7_Open_WeaponData"))
        {
            UnitTest_File.OpenMashal(path + "4_Binary.data");
        }
    }
}
