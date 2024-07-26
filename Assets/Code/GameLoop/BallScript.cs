using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public bool isBalling = false;
    public float MoveSpeed = 5f;
    public Rigidbody2D rb;
    [SerializeField]
    private Vector2 direction;
    private Vector3 lastPos;

    [SerializeField]
    private float counter = 0.1f;
    [SerializeField]
    private float AntiStuckCounter = 0.1f;

    [SerializeField]
    private SpriteRenderer _sprite;

    private void Start()
    {
        if (AppManager.theme)
        {
            _sprite.color = AppManager.theme.ballColor;
            GetComponent<TrailRenderer>().startColor = AppManager.theme.ballColor;
        }
            
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isBalling)
            rb.velocity = MoveSpeed * direction;


        Vector2 pos = transform.position;

        if (Helper.FastDistance(pos, lastPos) <= 0.05)
        {
            counter -= Time.fixedDeltaTime;
            if (counter <= 0)
                UnStick();
        }
        else
            counter = AntiStuckCounter;

        lastPos = transform.position;
    }

    public void Recall()
    {
        BallThrower.Instance.enabledBalls--;
        gameObject.SetActive(false);
    }

    public void StartBalling(Vector2 direction)
    {
        isBalling = true;
        this.direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 surfaceNormal = collision.GetContact(0).normal;

        direction = Vector2.Reflect(direction, surfaceNormal).normalized;

        // Avoid getting stuck by ensuring the direction is not too parallel to the surface
        if (Mathf.Abs(direction.y) < 0.05f)
        {
            // Adjust the direction slightly to avoid being parallel to the surface
            float adjustmentAngle = 10f; // Small adjustment angle in degrees
            direction = Quaternion.Euler(0, 0, adjustmentAngle) * direction;
            direction = direction.normalized;
        }
    }

    private void UnStick()
    {
        direction *= -1;
        counter = AntiStuckCounter;
        Debug.Log("Anti-Stick Triggered!");
    }
}
