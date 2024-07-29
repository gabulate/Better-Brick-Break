using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameMode", menuName = "Game Mode")]
public class GameModeSO : ScriptableObject
{
    public string gameMode = "";

    [Header("Play Area")]
    public Vector2 gridSize;

    [Header("Game")]
    public int initialMaxBlockValue = 2;
    public float difficultyScaling = 1;
    public int maxBlocksPerRow = 4;

    [Range(0, 1)]
    public float extraBallChance = 0.3f;
}
