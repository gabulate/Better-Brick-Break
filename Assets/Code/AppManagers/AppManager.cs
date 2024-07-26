using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    public AppAssetsSO appAssets;

    public static GameModeSO mode;

    public static float volume = 1;

    public static ThemeSO theme;


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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        foreach (ThemeSO t in appAssets.themes)
        {
            if (t.theme == SaveSystem.csd.theme)
            {
                theme = t;
                break;
            }
        }

        GameEvents.e_themeChanged.Invoke(theme);
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
    public static void SetGameMode(GameModeSO mode)
    {
        AppManager.mode = mode;
    }
}
