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

    // Start is called before the first frame update
    void Start()
    {
        StartBalling(new Vector2(0.3f, 0.7f));          
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

    

    public void StartBalling(Vector2 direction)
    {
        isBalling = true;
        this.direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the normal vector of the surface the object collided with
        Vector2 surfaceNormal = collision.GetContact(0).normal;

        direction = Vector2.Reflect(direction, surfaceNormal).normalized;

        //Avoids getting stuck
        while(Mathf.Abs(direction.y) < 0.05)
        {
            direction.y *= 1.2f;
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
