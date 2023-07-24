using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void save(GameData data)
    {
        string path = Application.persistentDataPath + "/data.qnd";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(path, FileMode.Create);
        formatter.Serialize(fs, data);
        fs.Close();
        Debug.LogError($"Saved {data.Language}");
    }

    public static GameData Load()
    {
        if (!File.Exists(GetPath()))
        {
            GameData emptyData = new GameData();
            save(emptyData);
            return emptyData;
        }


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(GetPath(), FileMode.Open);
        GameData data = formatter.Deserialize(fs) as GameData;
        fs.Close();

        return data;
    }

    private static string GetPath()
    {
        return Application.persistentDataPath + "/data.qnd";
    }

}
