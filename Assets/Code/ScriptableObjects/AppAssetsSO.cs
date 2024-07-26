using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AppAssets", menuName = "App Assets")]
public class AppAssetsSO : ScriptableObject
{
    public List<ThemeSO> themes;
    public List<GameModeSO> gameModes;
}
