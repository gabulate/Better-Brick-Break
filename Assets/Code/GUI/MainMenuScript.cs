using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Menu References")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject themesMenu;
    public GameObject gamemodesMenu;
    public GameObject aboutPage;
    public GameObject statsPage;

    [Header("Other References")]
    public TextMeshProUGUI statsText;
    public Slider volumeSlider;
    public Toggle trailToggle;
    public Toggle particleToggle;

    [Header("Themes")]
    public GameObject themeOptionPrefab;
    public GameObject themesParent;
    public void ShowPage(GameObject page)
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gamemodesMenu.SetActive(false);
        themesMenu.SetActive(false);
        aboutPage.SetActive(false);
        statsPage.SetActive(false);

        page.SetActive(true);
    }

    public void Play(GameModeSO mode)
    {
        AppManager.mode = mode;
        SaveSystem.csd.gamesPlayed++;
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        ShowPage(mainMenu);
        LoadThemes();
        LoadStats();
        LoadSettings();
    }

    public void Donate()
    {
        AppManager.Instance.GetComponent<MyStoreListener>().Donate();
    }

    private void LoadSettings()
    {
        volumeSlider.value = SaveSystem.csd.volume;
        trailToggle.isOn = SaveSystem.csd.showTrails;
        particleToggle.isOn = SaveSystem.csd.showParticles;
    }

    private void LoadStats()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("Total matches played: {0}\n", SaveSystem.csd.gamesPlayed);
        stringBuilder.AppendFormat("Total turns played: {0}\n", SaveSystem.csd.turnsPlayed);
        stringBuilder.AppendFormat("Total balls thrown: {0}\n", SaveSystem.csd.ballsThrown);
        stringBuilder.AppendFormat("Total blocks broken: {0}\n", SaveSystem.csd.brokenBlocks);
        stringBuilder.AppendFormat("Max blocks hit by a single ball: {0}\n", SaveSystem.csd.maxHitsBall);
        stringBuilder.AppendFormat("Maximum turns played: {0}\n", SaveSystem.csd.maxTurns);
        stringBuilder.AppendFormat("Maximum balls: {0}\n\n", SaveSystem.csd.maxBalls);
        stringBuilder.Append("Max scores by game mode:\n");

        var scores = SaveSystem.csd.maxScores;

        for (int i = 0; i < scores.Count; i++)
        {
            stringBuilder.AppendFormat("{0}: {1}\n", scores[i].key, scores[i].value);
        }

        statsText.text = stringBuilder.ToString();
    }

    private void LoadThemes()
    {
        List<ThemeSO> themes = AppManager.Instance.appAssets.themes;

        for (int i = 0; i < themes.Count; i++)
        {
            GameObject t = Instantiate(themeOptionPrefab, themesParent.transform);
            t.GetComponent<ThemeOption>().SetValues(themes[i]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPage(mainMenu);
        }

        //To do: if the player is making a custom game, dont go back all the way
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    public void ChangeVolume(float volume)
    {
        SaveSystem.csd.volume = volume;
        SaveSystem.Save();
    }

    public void ToggleTrails(bool toggle)
    {
        SaveSystem.csd.showTrails = toggle;
        SaveSystem.Save();
    }

    public void ToggleParticles(bool toggle)
    {
        SaveSystem.csd.showParticles = toggle;
        SaveSystem.Save();
    }
}
