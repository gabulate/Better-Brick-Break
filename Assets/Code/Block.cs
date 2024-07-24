using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Block : MonoBehaviour
{
    public int number = 1;
    [SerializeField]
    private TextMeshPro _text;
    [SerializeField]
    private SpriteRenderer _sprite;

    public void SetBlock(int number)
    {
        this.number = number;
        _text.text = number.ToString();

        _sprite.color = AssetsHolder.Instance.bColors.Evaluate(number / 15f).gamma;
    }

    private void Start()
    {
        SetBlock(number);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        number--;
        if (number <= 0)
            BreakBlock();
        else
            SetBlock(number);
    }

    private void BreakBlock()
    {
        gameObject.SetActive(false);
    }
}
