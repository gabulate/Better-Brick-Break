using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        InitialiseSaveData();
    }

    private void InitialiseSaveData()
    {
        try
        {
            if (!SaveSystem.LoadSaveFromDisk())
            {
                SaveSystem.CreateNewSave();
            }
        }
        catch (Exception)
        {
            SaveSystem.CreateNewSave();
            throw;
        }
    }
}
