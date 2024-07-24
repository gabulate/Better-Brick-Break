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
}
