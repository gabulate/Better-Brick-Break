using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject ThemesMenu;
    public GameObject GamemodesMenu;



    public void PlayClassic()
    {
        SceneManager.LoadScene("Game");
    }

}
