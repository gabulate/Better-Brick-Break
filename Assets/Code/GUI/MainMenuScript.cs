using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        page.SetActive(true);
    }

    public void Play(GameModeSO mode)
    {
        AppManager.mode = mode;
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        ShowPage(mainMenu);
        LoadThemes();
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
}
