using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static readonly string SAVE_FOLDER = Application.persistentDataPath + "/save/";
    private static readonly string SAVE_FILE = "savedata.json";

    public static void SaveData(SaveData data)
    {
        string filePath = SAVE_FOLDER + SAVE_FILE;
        string json = JsonUtility.ToJson(data);

        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        File.WriteAllText(filePath, json);
    }

    public static SaveData LoadData()
    {
        string filePath = SAVE_FOLDER + SAVE_FILE;

        if (!File.Exists(filePath))
        {
            return null;
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
