using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPickUp : Block
{
    public override void SetBlock(int number, int[] gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.extraBallsNextTurn++;
        BreakBlock();
    }

    public override void BreakBlock()
    {
        GameEvents.e_blockBroke.Invoke(gridPosition[0], gridPosition[1]);
        Destroy(gameObject);
    }

    protected override void OnEnable()
    {
        transform.localScale = Vector3.zero;
        if (AppManager.theme)
            _sprite.color = AppManager.theme.ballColor;

        StartCoroutine(ShowBlock(0.2f));
    }
}
