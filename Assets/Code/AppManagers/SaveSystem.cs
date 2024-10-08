using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SaveSystem
{
    public static SaveData csd;
    private static readonly string path = Application.persistentDataPath + "/saveFile.json";
    //C:\Users\Gabri\AppData\LocalLow\Gabulate\Structured Defenders

    public static bool LoadSaveFromDisk()
    {
        try
        {
            if (!File.Exists(path))
                return false;

            string json = File.ReadAllText(path);
            csd = JsonUtility.FromJson<SaveData>(json);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            throw e;
        }
    }

    public static void CreateNewSave()
    {
        csd = new SaveData();
        Save();
    }

    public static bool Save()
    {
        try
        {
            string json = JsonUtility.ToJson(csd);

            File.WriteAllText(path, json);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public static void DeleteSave()
    {
        File.Delete(path);
        CreateNewSave();
        Debug.Log("Save data has been deleted!");
    }
}
