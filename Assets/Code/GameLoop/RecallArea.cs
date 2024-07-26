using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallArea : MonoBehaviour
{
    static readonly LayerMask ballLayer = 1 << 6;
    public bool isRecalling = false;
    public bool firstBallRecalled = false;
    public float newPosition = 0;
    private int totalBalls = 0;
    public int currentRecalledBalls = 0;

    private void Start()
    {
        if (AppManager.theme)
            AssetsHolder.Instance.ballVisual.GetComponent<SpriteRenderer>().color = AppManager.theme.ballColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRecalling && ballLayer == (ballLayer | (1 << collision.gameObject.layer)))
        {
            collision.GetComponent<BallScript>().Recall();
            currentRecalledBalls++;

            if (currentRecalledBalls >= totalBalls)
                StopRecalling();

            if (firstBallRecalled)
                return;

            //Sets the ball thrower at the new position and enables the ball visual
            newPosition = collision.gameObject.transform.position.x;
            BallThrower.Instance.MovePosition(newPosition);
            firstBallRecalled = true;
            AssetsHolder.Instance.ballVisual.SetActive(true);
        }
    }

    private void StopRecalling()
    {
        isRecalling = false;
        GameEvents.e_StoppedRecalling.Invoke();
    }

    private void StartRecalling(int totalBalls)
    {
        isRecalling = true;
        firstBallRecalled = false;
        this.totalBalls = totalBalls;
        currentRecalledBalls = 0;
        AssetsHolder.Instance.ballVisual.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.e_StartedThrowing.AddListener(StartRecalling);
    }

    private void OnDisable()
    {
        GameEvents.e_StartedThrowing.RemoveListener(StartRecalling);
    }
}
