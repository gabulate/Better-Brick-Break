using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI lastScoreText;
    public List<TextMeshProUGUI> labels;
    public GameObject pauseMenu;
    public Image pauseButton;
    public GameObject gameOverModal;
    public GameObject allClearText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        UpdateLastScore(SaveSystem.csd.lastScore);
        UpdateMaxScore(SaveSystem.csd.maxScores.Get(AppManager.mode.gameMode));

        if (AppManager.theme)
        {
            scoreText.color = AppManager.theme.ballColor;
            maxScoreText.color = AppManager.theme.ballColor;
            lastScoreText.color = AppManager.theme.ballColor;
            pauseButton.color = AppManager.theme.ballColor;

            foreach(var label in labels)
            {
                label.color = AppManager.theme.ballColor;
            }
        }
    }

    public void ReturnToMenu()
    {
        AppManager.mode = null;
        SceneManager.LoadScene(0);
    }

    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void ShowGameOverModal() => gameOverModal.GetComponent<Animator>().SetTrigger("show");
    public void ShowAllClearText() => allClearText.GetComponent<Animator>().SetTrigger("show");

    public void ShowPauseMenu(bool enabled)
    {
        if(!GameManager.gameLost)
        {
            pauseMenu.SetActive(enabled);
            GameEvents.e_gamePaused.Invoke(enabled);
        }
    }

    public void UpdateScore(uint score) => scoreText.text = score.ToString();

    public void UpdateMaxScore(uint score) => maxScoreText.text = score.ToString();

    public void UpdateLastScore(uint score) => lastScoreText.text = score.ToString();

    private void OnEnable()
    {
        GameEvents.e_scoreChanged.AddListener(UpdateScore);
        GameEvents.e_lastScoreChanged.AddListener(UpdateLastScore);
        GameEvents.e_maxScoreChanged.AddListener(UpdateMaxScore);
        GameEvents.e_gameLost.AddListener(ShowGameOverModal);
        GameEvents.e_allClear.AddListener(ShowAllClearText);
    }

    private void OnDisable()
    {
        GameEvents.e_scoreChanged.RemoveListener(UpdateScore);
        GameEvents.e_lastScoreChanged.RemoveListener(UpdateLastScore);
        GameEvents.e_maxScoreChanged.RemoveListener(UpdateMaxScore);
        GameEvents.e_gameLost.RemoveListener(ShowGameOverModal);
        GameEvents.e_allClear.RemoveListener(ShowAllClearText);
    }
}
