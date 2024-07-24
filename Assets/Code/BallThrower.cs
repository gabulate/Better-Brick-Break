using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public static BallThrower Instance;
    public static bool isThrowing = false;
    public int currentBalls = 1;

    public int totalBalls = 20;
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
        balls = new List<BallScript>();
        for (int i = 0; i < initialBalls; i++)
        {
            GameObject obj = Instantiate(AssetsHolder.Instance.ballPrefab);
            obj.SetActive(false);
            balls.Add(obj.GetComponent<BallScript>());
        }
    }

    public void StartThrowing(Vector2 direction)
    {
        Debug.Log("Threw!");
        StartCoroutine(ThrowBalls(direction));
    }

    private IEnumerator ThrowBalls(Vector2 direction)
    {
        for (int i = 0; i < currentBalls; i++)
        {
            BallScript ball = GetBall();
            ball.transform.position = transform.position;
            ball.gameObject.SetActive(true);
            enabledBalls++;
            ball.StartBalling(direction);
            yield return new WaitForSeconds(secsBetween);
        }
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
            totalBalls++;
            return ball;
        }

        return null;
    }
}
