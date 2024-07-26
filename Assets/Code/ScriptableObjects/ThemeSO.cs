using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Theme")]
public class ThemeSO : ScriptableObject
{
    public string theme = "Theme name";

    [Header("Play Area")]
    public Color backgroundColor = Color.white;
    public Color playAreaColor = Color.white;
    public Color borderColor = Color.white;

    [Header("Blocks")]
    public Gradient blockColors;
    public Sprite blockSprite;
    public Color blockTextColor = Color.black;

    [Header("Balls")]
    public Color ballColor = Color.white;
}
