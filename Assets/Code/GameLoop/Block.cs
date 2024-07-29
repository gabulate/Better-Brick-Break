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
    protected SpriteRenderer _sprite;

    public int[] gridPosition;

    public virtual void SetBlock(int number, int[] gridPosition)
    {
        this.gridPosition = gridPosition;
        this.number = number;
        _text.text = number.ToString();

        SetColor(number);
    }

    public void SetColor(int number)
    {
        if (!AppManager.theme)
            return;

        float colorNumber = number;
        while (colorNumber > AppManager.theme.maxColorAt)
            colorNumber -= AppManager.theme.maxColorAt;

        _sprite.color = AssetsHolder.Instance.bColors.Evaluate(colorNumber / AppManager.theme.maxColorAt).gamma;

            _text.color = AppManager.theme.blockTextColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        number--;
        if (number <= 0)
            BreakBlock();
        else
            SetBlock(number, gridPosition);
    }

    public virtual void BreakBlock()
    {
        GameEvents.e_blockBroke.Invoke(gridPosition[0], gridPosition[1]);
        var pgo = Instantiate(AssetsHolder.Instance.destroyParticles, transform.position, Quaternion.identity);
        var p = pgo.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = p.main;
        main.startColor = _sprite.color;
        p.Play();

        GameManager.Instance.AddScore(1);

        Destroy(gameObject);
    }

    public IEnumerator MovePosition(Vector3 position, float seconds)
    {
        Vector3 previousPosition = transform.position;

        float step = 1 / seconds;
        float current = 0;

        while (current < 1)
        {
            current += step * Time.deltaTime;
            transform.position = Vector3.Lerp(previousPosition, position, current);
            yield return null;
        }
    }

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;
        if (AppManager.theme)
            _sprite.sprite = AppManager.theme.blockSprite;

        StartCoroutine(ShowBlock(0.2f));
    }

    protected IEnumerator ShowBlock(float seconds)
    {
        yield return new WaitForSeconds(0.4f);

        Vector3 targetSize = Vector3.one * 0.95f;

        float step = 1 / seconds;
        float current = 0;

        while (current < 1)
        {
            current += step * Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetSize, current);
            yield return null;
        }

        transform.localScale = targetSize;

    }
}
