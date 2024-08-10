using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent<uint> e_StartedThrowing = new UnityEvent<uint>();
    public static UnityEvent e_StoppedRecalling = new UnityEvent();
    public static UnityEvent e_gameLost = new UnityEvent();
    public static UnityEvent<int,int> e_blockBroke = new UnityEvent<int,int>();

    public static UnityEvent<uint> e_scoreChanged = new UnityEvent<uint>();
    public static UnityEvent<uint> e_maxScoreChanged = new UnityEvent<uint>();
    public static UnityEvent<uint> e_lastScoreChanged = new UnityEvent<uint>();
    public static UnityEvent<bool> e_gamePaused = new UnityEvent<bool>();
    public static UnityEvent e_allClear = new UnityEvent();

    public static UnityEvent<ThemeSO> e_themeChanged = new UnityEvent<ThemeSO>();
}
