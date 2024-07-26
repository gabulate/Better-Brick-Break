using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private SpriteRenderer playArea;
    [SerializeField]
    private SpriteRenderer border;

    private void Start()
    {
        if (AppManager.theme)
            ThemeManager.Instance.SetTheme(AppManager.theme);
    }

    public void SetTheme(ThemeSO theme)
    {
        AppManager.theme = theme;
        AssetsHolder.Instance.bColors = theme.blockColors;

        SaveSystem.csd.theme = theme.name;
        SaveSystem.Save();

        _cam.backgroundColor = theme.backgroundColor;
        playArea.color = theme.playAreaColor;
        border.color = theme.borderColor;

        foreach(Block b in FindObjectsOfType<Block>())
        {
            if(b.GetType() != typeof(BallPickUp))
            {
                b.SetColor(b.number);

                SpriteRenderer s = b.GetComponent<SpriteRenderer>();

                s.sprite = theme.blockSprite;
            }
        }

        foreach(BallScript b in FindObjectsOfType<BallScript>())
        {
            SpriteRenderer s = b.GetComponent<SpriteRenderer>();
            s.color = theme.ballColor;
        }
    }

    private void OnEnable() => GameEvents.e_themeChanged.AddListener(SetTheme);
    private void OnDisable() => GameEvents.e_themeChanged.RemoveListener(SetTheme);

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
