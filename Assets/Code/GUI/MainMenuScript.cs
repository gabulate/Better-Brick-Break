using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject themesMenu;
    public GameObject gamemodesMenu;
    public GameObject aboutPage;

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

}
