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

    internal void SpawnRow(int maxValue, int maxBlocks)
    {
        float ballChance = Random.Range(0, 1f);
        if (ballChance <= GameManager.Instance.extraBallChance)
        {
            SpawnBallPickUp(Random.Range(0, hSize), 0);
        }

        for (int i = 0; i < maxBlocks; i++)
        {
            int cellX = Random.Range(0, hSize);
            if(!IsCellOcuppied(cellX, 0))
            {
                SpawnBlock(cellX, 0, Random.Range(1, maxValue));
            }
        }
    }

    private void SpawnBallPickUp(int x, int y)
    {
        if (IsCellOcuppied(x, y))
        {
            Debug.LogWarning("Tried to spawn on an occupied cell!\nCell: x:" + x + ", y:" + y);
            return;
        }

        //Spawn block
        GameObject g = Instantiate(AssetsHolder.Instance.ballPickUpPrefab, GridToPosition(x, y), Quaternion.identity, transform);
        Block bs = g.GetComponent<Block>();
        int[] pos = { x, y };
        bs.SetBlock(1, pos);

        //Set value to grid
        grid[x, y].block = bs;
    }

    public void SpawnBlock(int x, int y, int number)
    {
        if(IsCellOcuppied(x, y))
        {
            Debug.LogWarning("Tried to spawn on an occupied cell!\nCell: x:" + x + ", y:" + y);
            return;
        }

        //Spawn block
        GameObject g = Instantiate(AssetsHolder.Instance.blockPrefab, GridToPosition(x, y), Quaternion.identity, transform);
        Block bs = g.GetComponent<Block>();
        int[] pos = { x, y };   
        bs.SetBlock(number, pos);

        //Set value to grid
        grid[x, y].block = bs;
    }

    public void MoveBlocksDown()
    {
        //Check if there are any blocks on the final row, if there are lose the game
        for (int i = 0; i < hSize; i++)
        {
            if (IsCellOcuppied(i, vSize - 1))
            {
                if(grid[i, vSize-1].block.GetType() == typeof(BallPickUp)) //If the block is actually a ball pickup don't lose
                {
                    grid[i, vSize - 1].block.BreakBlock();
                    continue;
                }

                GameEvents.e_gameLost.Invoke();
                MoveFinalBlocksDown();
                return;
            }
        }

        for (int i = 0; i < hSize; i++)
        {
            for (int j = vSize - 1; j >= 0; j--)
            {
                if(IsCellOcuppied(i, j))
                {  
                    grid[i, j + 1].block = grid[i, j].block;
                    StartCoroutine(grid[i, j].block.MovePosition(grid[i, j + 1].worldPos, 0.5f));
                    grid[i, j].block.gridPosition = new int[]{ i, j + 1 };
                    grid[i, j].block = null;
                }
            }
        }
    }

    private void MoveFinalBlocksDown()
    {
        for (int i = 0; i < hSize; i++)
        {
            for (int j = vSize - 1; j >= 0; j--)
            {
                if (IsCellOcuppied(i, j))
                {
                    Vector3 targetPos = grid[i, j].worldPos;
                    targetPos.y -= 1;

                    StartCoroutine(grid[i, j].block.MovePosition(targetPos, 0.5f));
                }
            }
        }
    }

    private void OnBlockBroke(int x, int y )
    {
        grid[x, y].block = null;
    }

    public Vector2 GridToPosition(int x, int y)
    {
        return grid[x, y].worldPos;
    }

    public bool IsCellOcuppied(int x, int y)
    {
        return grid[x, y].block;
    }

    private void OnEnable()
    {
        GameEvents.e_blockBroke.AddListener(OnBlockBroke);
    }

    private void OnDisable()
    {
        GameEvents.e_blockBroke.RemoveListener(OnBlockBroke);
    }
}

[System.Serializable]
public struct GridCell
{
    public Vector2 worldPos;
    public Block block;
}
