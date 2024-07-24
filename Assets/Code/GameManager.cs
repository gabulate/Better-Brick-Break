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

    // Start is called before the first frame update
    void Start()
    {
        BlockGrid.Instance.GenerateGrid((int)gridSize.x, (int)gridSize.y);

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                BlockGrid.Instance.SpawnBlock(i, j, 5);
            }
        }
        
        SetPlayableArea();    
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
}
