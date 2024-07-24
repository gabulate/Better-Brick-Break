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

    [Header("Game")]
    public static bool canThrow = true;
    public int maxBlockValue = 2;
    public int maxBlocksPerRow = 4;
    [Range(0, 1)]
    public float extraBallChance = 0.3f;

    public int extraBallsNextTurn = 0;

    // Start is called before the first frame update
    void Start()
    {
        BlockGrid.Instance.GenerateGrid((int)gridSize.x, (int)gridSize.y);

        BlockGrid.Instance.SpawnRow(maxBlockValue, maxBlocksPerRow);

        SetPlayableArea();    
    }

    private void OnStoppedRecalling()
    {
        BlockGrid.Instance.MoveBlocksDown();
        maxBlockValue++;
        BlockGrid.Instance.SpawnRow(maxBlockValue, maxBlocksPerRow);
        BallThrower.Instance.currentBalls += extraBallsNextTurn;
        extraBallsNextTurn = 0;
    }

    private void OnGameLost()
    {
        Debug.Log("Game Lost!");
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

    private void OnEnable()
    {
        GameEvents.e_StartedThrowing.AddListener(OnStartedThrowing);
        GameEvents.e_StoppedRecalling.AddListener(OnStoppedRecalling);
        GameEvents.e_gameLost.AddListener(OnGameLost);
    }

    private void OnDisable()
    {
        GameEvents.e_StartedThrowing.RemoveListener(OnStartedThrowing);
        GameEvents.e_StoppedRecalling.RemoveListener(OnStoppedRecalling);
        GameEvents.e_gameLost.RemoveListener(OnGameLost);
    }
}
