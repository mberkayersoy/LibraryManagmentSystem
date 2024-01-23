using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class SaveLoadManager<T> : MonoBehaviour
{
    private static readonly string savePath = "Assets/Resources/";

    public static void Save(Dictionary<string, T> data, string fileName)
    {
        string filePath = savePath + fileName;

        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(filePath, json);
    }
    public static Dictionary<string,T> Load(string fileName)
    {
        string filePath = savePath + fileName;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, T>>(json);
        }
        else
        {
            return new Dictionary<string, T>();
        }
    }
}
