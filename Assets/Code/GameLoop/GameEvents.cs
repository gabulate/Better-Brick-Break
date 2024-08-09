using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent<int> e_StartedThrowing = new UnityEvent<int>();
    public static UnityEvent e_StoppedRecalling = new UnityEvent();
    public static UnityEvent e_gameLost = new UnityEvent();
    public static UnityEvent<int,int> e_blockBroke = new UnityEvent<int,int>();

    public static UnityEvent<int> e_scoreChanged = new UnityEvent<int>();
    public static UnityEvent<int> e_maxScoreChanged = new UnityEvent<int>();
    public static UnityEvent<int> e_lastScoreChanged = new UnityEvent<int>();
    public static UnityEvent<bool> e_gamePaused = new UnityEvent<bool>();
    public static UnityEvent e_allClear = new UnityEvent();

    public static UnityEvent<ThemeSO> e_themeChanged = new UnityEvent<ThemeSO>();
}
