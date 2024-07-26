using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeOption : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;

    public ThemeSO theme;

    public void SetValues(ThemeSO theme)
    {
        this.theme = theme;

        image.sprite = theme.blockSprite;
        image.color = theme.blockColors.colorKeys[0].color;

        text.text = theme.theme;
        text.color = theme.blockTextColor;

        GetComponent<Button>().onClick.AddListener(SetTheme);
    }

    public void SetTheme()
    {
        ThemeManager.Instance.SetTheme(theme);
    }
}
