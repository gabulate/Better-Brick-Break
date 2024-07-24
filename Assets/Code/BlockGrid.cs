using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrid : MonoBehaviour
{
    public static BlockGrid Instance;
    public static int hSize = 6;
    public static int vSize = 8;

    [SerializeField]
    private GridCell[,] grid;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void GenerateGrid(int x, int y)
    {
        hSize = x;
        vSize = y;
        transform.position = new Vector3(0, vSize / 2, 0);

        grid = new GridCell[x, y];

        float xFactor = x / 2;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float xPos = Mathf.Lerp(-xFactor, xFactor, i / (float)x) + 0.5f;
                float yPos = Mathf.Lerp(0, -vSize, j / (float)y) - 0.5f;

                grid[i, j] = new GridCell
                {
                    block = null,
                    worldPos = new Vector2(xPos, yPos) + (Vector2)transform.position
                };
            }
        }
    }

    public void SpawnBlock(int x, int y, int number)
    {
        GameObject g = Instantiate(AssetsHolder.Instance.blockPrefab, GridToPosition(x, y), Quaternion.identity, transform);
        g.GetComponent<Block>().SetBlock(number);
    }

    public Vector2 GridToPosition(int x, int y)
    {
        return grid[x, y].worldPos;
    }
}

public struct GridCell
{
    public Vector2 worldPos;
    public Block block;
}
