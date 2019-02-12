﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownApple : MonoBehaviour
{

    public Vector3 startPos;
    public Vector3 firstPos;
    public Vector3 endPos;
    public GameObject boundsUp;
    public float radiusOfCircle;
    public GameObject boundsDown;

    public float moveDownSpeedIncrease;
    public float moveDownSpeed;
    public float moveUpSpeedDecrease;
    public float moveUpSpeed;
    public float frequency;
    public float magnitude;
    public Vector3 direction;

    Vector3 velocity;

    bool movingUp = true;
    bool movingDown = false;
    public float colliderRadius;

    public float waitAmount;
    float waitTimer;

    public Vector3 whereIamGoing;
    float startTime;
    // Use this for initialization
    void Start()
    {
        whereIamGoing = firstPos;
        waitTimer = waitAmount;
        startPos = transform.position;
        startTime = Time.time;
        firstPos = new Vector3(Random.Range(boundsUp.GetComponent<Collider2D>().bounds.min.x, boundsUp.GetComponent<Collider2D>().bounds.max.x)
            , Random.Range(boundsUp.GetComponent<Collider2D>().bounds.min.y, boundsUp.GetComponent<Collider2D>().bounds.max.y), 0);
        bool canUse = false;
        int index = 0;
        while (!canUse)
        {
            endPos = new Vector3(firstPos.x, Random.Range(boundsDown.GetComponent<Collider2D>().bounds.min.y, boundsDown.GetComponent<Collider2D>().bounds.max.y), 0);
            index++;
            if (index >= 100)
            {
                canUse = true;
            }
            else
            {
                if (Physics2D.OverlapCircle(endPos, colliderRadius))
                {
                    index++;
                    continue;
                }
                else
                {
                    canUse = true;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + (transform.right * Mathf.Sin(Time.time * frequency) * magnitude);

        if (movingUp)
        {
            transform.position = Vector3.Slerp(startPos, firstPos, (Time.time - startTime) * moveUpSpeed);
            moveUpSpeed -= moveUpSpeedDecrease;
            if (Vector3.Distance(transform.position, firstPos) < 1.0f)
            {
                movingUp = false;
                whereIamGoing = endPos;
            }
        }
        else if (!movingUp && !movingDown)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                movingDown = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, moveDownSpeed * Time.deltaTime);
            moveDownSpeed += moveDownSpeedIncrease;
            if (Vector3.Distance(transform.position, endPos) < 1.0f)
            {
                Physics2D.OverlapCircle(transform.position, colliderRadius);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}