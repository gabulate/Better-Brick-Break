using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallThrower : MonoBehaviour
{
    public static BallThrower Instance;
    public TextMeshPro ballsText;
    public static bool isThrowing = false;
    public int currentBalls = 1;
    public int remainingBalls = 1;

    public int totalPooledBalls = 0;
    public int initialBalls = 20;
    public bool Expandable = false;
    public int maximumLenght = 30;
    public int enabledBalls = 0;

    public static List<BallScript> balls = new List<BallScript>();
    public float secsBetween = 0.1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        totalPooledBalls = 0;

        balls = new List<BallScript>();
        for (int i = 0; i < initialBalls; i++)
        {
            GameObject obj = Instantiate(AssetsHolder.Instance.ballPrefab);
            obj.SetActive(false);
            balls.Add(obj.GetComponent<BallScript>());
            totalPooledBalls++;
        }
    }

    public void MovePosition(float newPosition)
    {
        transform.position = new Vector3(
            newPosition,
            transform.position.y, 
            transform.position.z);
    }

    public void StartThrowing(Vector2 direction)
    {
        if (!GameManager.canThrow || GameManager.gameLost)
            return;

        if (direction.magnitude < 0.1)
        {
            direction.y = 1;
            direction.x = 0;
        }

        StartCoroutine(ThrowBalls(direction, transform.position));
        GameEvents.e_StartedThrowing.Invoke(currentBalls);

        GameManager.canThrow = false;
    }

    private IEnumerator ThrowBalls(Vector2 direction, Vector3 position)
    {
        remainingBalls = currentBalls;
        for (int i = 0; i < currentBalls; i++)
        {
            BallScript ball = GetBall();
            ball.transform.position = position;
            ball.gameObject.SetActive(true);
            enabledBalls++;
            ball.StartBalling(direction);

            remainingBalls--;
            if(remainingBalls == 0)
                ballsText.enabled = false;
            else
                ballsText.text = "x" + remainingBalls;

            yield return new WaitForSeconds(secsBetween);
        }

    }

    private void EnableThrowing()
    {
        if(!GameManager.gameLost)
            StartCoroutine(EnableThrowingCoroutine(0.5f));
    }

    private IEnumerator EnableThrowingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameManager.canThrow = true;
        ballsText.enabled = true;
        ballsText.text = "x" + currentBalls;
    }

    public BallScript GetBall()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (!balls[i].gameObject.activeInHierarchy)
            {
                return balls[i];
            }
        }

        if (Expandable && balls.Count < maximumLenght)
        {
            GameObject obj = Instantiate(AssetsHolder.Instance.ballPrefab);
            obj.SetActive(false);
            BallScript ball = obj.GetComponent<BallScript>();
            balls.Add(ball);
            totalPooledBalls++;
            return ball;
        }

        return null;
    }

    private void OnEnable()
    {
        GameEvents.e_StoppedRecalling.AddListener(EnableThrowing);
    }

    private void OnDisable()
    {
        GameEvents.e_StoppedRecalling.RemoveListener(EnableThrowing);
    }
}
