using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI lastScoreText;
    public GameObject pauseMenu;
    public GameObject gameOverModal;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        UpdateLastScore(SaveSystem.csd.lastScore);
        UpdateMaxScore(SaveSystem.csd.maxScores.Get(AppManager.mode.gameMode));
    }

    public void ReturnToMenu()
    {
        AppManager.mode = null;
        SceneManager.LoadScene(0);
    }

    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void ShowGameOverModal() => gameOverModal.GetComponent<Animator>().SetTrigger("show");

    public void ShowPauseMenu(bool enabled)
    {
        if(!GameManager.gameLost)
        {
            pauseMenu.SetActive(enabled);
            GameEvents.e_gamePaused.Invoke(enabled);
        }
    }

    public void UpdateScore(int score) => scoreText.text = score.ToString();

    public void UpdateMaxScore(int score) => maxScoreText.text = score.ToString();

    public void UpdateLastScore(int score) => lastScoreText.text = score.ToString();

    private void OnEnable()
    {
        GameEvents.e_scoreChanged.AddListener(UpdateScore);
        GameEvents.e_lastScoreChanged.AddListener(UpdateLastScore);
        GameEvents.e_maxScoreChanged.AddListener(UpdateMaxScore);
        GameEvents.e_gameLost.AddListener(ShowGameOverModal);
    }

    private void OnDisable()
    {
        GameEvents.e_scoreChanged.RemoveListener(UpdateScore);
        GameEvents.e_lastScoreChanged.RemoveListener(UpdateLastScore);
        GameEvents.e_maxScoreChanged.RemoveListener(UpdateMaxScore);
        GameEvents.e_gameLost.RemoveListener(ShowGameOverModal);
    }
}
