using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Play Area")]
    public SpriteRenderer playArea;
    public Vector2 gridSize;
    public Transform RecallArea;

    [Header("Game")]
    public static bool gameLost = false;
    public static bool canThrow = true;
    public float maxBlockValue = 2;
    public int initialMaxBlockValue = 2;
    public float difficultyScaling = 1;
    public int maxBlocksPerRow = 4;
    public int score = 0;
    public int maxScore = 0;

    [Range(0, 1)]
    public float extraBallChance = 0.3f;

    public int extraBallsNextTurn = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(AppManager.mode)
            SetPlaySettings();

        gameLost = false;
        canThrow = true;
        maxBlockValue = initialMaxBlockValue;
        score = 0;
        BlockGrid.Instance.GenerateGrid((int)gridSize.x, (int)gridSize.y);

        BlockGrid.Instance.SpawnRow((int)maxBlockValue, maxBlocksPerRow);


        SetPlayableArea(); 
        Time.timeScale = 1;
    }

    private void SetPlaySettings()
    {
        gridSize = AppManager.mode.gridSize;
        initialMaxBlockValue = AppManager.mode.initialMaxBlockValue;
        difficultyScaling = AppManager.mode.difficultyScaling;
        maxBlocksPerRow = AppManager.mode.maxBlocksPerRow;
        extraBallChance = AppManager.mode.extraBallChance;
    }

    private void OnStoppedRecalling()
    {
        BlockGrid.Instance.MoveBlocksDown();
        maxBlockValue += difficultyScaling;
        BlockGrid.Instance.SpawnRow((int)maxBlockValue, maxBlocksPerRow);
        BallThrower.Instance.currentBalls += extraBallsNextTurn;
        extraBallsNextTurn = 0;

        score++;
        GameEvents.e_scoreChanged.Invoke(score);
        SaveSystem.csd.lastScore = score;


        if (score > SaveSystem.csd.maxScore)
        {
            GameEvents.e_maxScoreChanged.Invoke(score);
            SaveSystem.csd.maxScore = score;
            SaveSystem.Save();
        }
    }

    private void OnGameLost()
    {
        gameLost = true;
        canThrow = false;
    }

    private void OnStartedThrowing(int arg0)
    {
        //throw new NotImplementedException();
    }

    private void SetPlayableArea()
    {
        playArea.transform.localScale = new Vector3(BlockGrid.hSize, BlockGrid.vSize + 2, 1);
        BallThrower.Instance.transform.position = new Vector3(0, (-BlockGrid.vSize / 2) -0.5f, 0);
        RecallArea.position = new Vector3(0, (-BlockGrid.vSize / 2) -0.5f, 0);
        Camera cam = Camera.main;

        float w = playArea.bounds.size.x;
        float h = playArea.bounds.size.y;
        float x = w * 0.5f - 0.5f;
        float y = h * 0.5f - 0.5f;

        cam.transform.position = new Vector3(x, y, -10f);

        cam.orthographicSize = ((w > h * cam.aspect) ? (float)w / (float)cam.pixelWidth * cam.pixelHeight : h) / 2;
        cam.transform.position = new Vector3(playArea.transform.position.x, playArea.transform.position.y, -10);

        Debug.Log("Set camera size to: x:" + w + " y: " + h + "\nCamera size: "+ cam.orthographicSize);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void OnGamePaused(bool paused)
    {
        if (paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private void OnEnable()
    {
        GameEvents.e_StartedThrowing.AddListener(OnStartedThrowing);
        GameEvents.e_StoppedRecalling.AddListener(OnStoppedRecalling);
        GameEvents.e_gameLost.AddListener(OnGameLost);
        GameEvents.e_gamePaused.AddListener(OnGamePaused);
    }

    private void OnDisable()
    {
        GameEvents.e_StartedThrowing.RemoveListener(OnStartedThrowing);
        GameEvents.e_StoppedRecalling.RemoveListener(OnStoppedRecalling);
        GameEvents.e_gameLost.RemoveListener(OnGameLost);
        GameEvents.e_gamePaused.RemoveListener(OnGamePaused);
    }
}
