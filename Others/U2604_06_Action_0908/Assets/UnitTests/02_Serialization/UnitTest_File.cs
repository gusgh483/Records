using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System;

public class UnitTest_File
{
    public static void SaveJson(string path, PlayerData playerData)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine($"\t\"Name\": \"{playerData.Name}\",");
        stringBuilder.AppendLine($"\t\"HP\": \"{playerData.HP}\",");
        stringBuilder.AppendLine($"\t\"Attack\": \"{playerData.Attack}\",");
        stringBuilder.AppendLine($"\t\"Defence\": \"{playerData.Defence}\",");
        stringBuilder.AppendLine("}");

        File.WriteAllText(path, stringBuilder.ToString());

        Debug.Log($"{path}에 json 파일 저장");
        AssetDatabase.Refresh();
    }

    public static void SaveJson2(string path, PlayerData playerData)
    {
        string str = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        Debug.Log(str);

        File.WriteAllText(path, str);

        Debug.Log($"{path}에 json 파일 저장");
        AssetDatabase.Refresh();
    }

    public static void OpenJson(string path)
    {
        string str = File.ReadAllText(path);

        PlayerData data = JsonConvert.DeserializeObject<PlayerData>(str);

        str += "\n\n";
        str += "Name : " + data.Name + "\n";
        str += "HP : " + data.HP + "\n";
        str += "Attack : " + data.Attack + "\n";
        str += "Defence : " + data.Defence;

        EditorUtility.DisplayDialog("Json", str, "확인");
    }

    public static void SaveBinaryFile(string path, PlayerData data)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(data.Name);
                writer.Write(data.HP);
                writer.Write(data.Attack);
                writer.Write(data.Defence);
            } //writer.Close()
        } //fileStream.Close()

        Debug.Log($"{path}에 data 파일 저장");
        AssetDatabase.Refresh();
    }

    public static void OpenBinaryFile(string path)
    {
        string str = "";

        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                str += "Name : " + reader.ReadString() + "\n";
                str += "HP : " + reader.ReadSingle() + "\n";
                str += "Attack : " + reader.ReadSingle() + "\n";
                str += "Defence : " + reader.ReadSingle();
            } //writer.Close()
        } //fileStream.Close()

        EditorUtility.DisplayDialog("Binary", str, "확인");
    }

    public static void SaveMashal(string path, WeaponData[] datas)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(datas.Length);

                ReadOnlySpan<WeaponData> span = datas;
                ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(span);

                writer.Write(bytes);
            } //writer.Close()
        } //fileStream.Close()

        Debug.Log($"{path}에 data 파일 저장");
        AssetDatabase.Refresh();
    }

    public static void OpenMashal(string path)
    {
        string str = "";
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                int count = reader.ReadInt32();

                int byteSize = Marshal.SizeOf<WeaponData>();
                Debug.Log($"WeaponData Size : {byteSize}");
                
                int totalBytes = count * byteSize;
                Debug.Log($"Total Byte Size : {totalBytes}");

                byte[] buffer = reader.ReadBytes(totalBytes);

                ReadOnlySpan<byte> span = buffer;
                ReadOnlySpan<WeaponData> datas = MemoryMarshal.Cast<byte, WeaponData>(span);

                WeaponData[] weaponDatas = datas.ToArray();


                foreach(WeaponData data in weaponDatas)
                {
                    str += "----------------------------\n";
                    str += "Type : " + data.Type + "\n";
                    str += "Power : " + data.Power + "\n";
                    str += "Speed : " + data.Speed + "\n";
                    str += "Durability : " + data.Durability+ "\n";
                    str += "CriticalRatio : " + data.CriticalRatio + "\n";
                }

            } //writer.Close()
        } //fileStream.Close()

        EditorUtility.DisplayDialog("Binary", str, "확인");
    }
}
