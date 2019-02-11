using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoam : MonoBehaviour
{
    public float health;
    public float timeBetweenMovements;
    float timeBetweenMovementsTimer;

    public float moveDistance;

    public float moveSpeed;

    Vector3 targetPosition;
    GameObject player;

    bool isCharging;
    bool isMoving;

    Vector3 chargePoint;

    Vector3 velocity = Vector3.zero;
    bool isMovingRandomly = true;
    public float followSpeed;
    public float distanceNear;
    private Vector3 previousRand;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < distanceNear)
        {
            isMovingRandomly = false;
        }

        if (isMovingRandomly)
        {
            if (isCharging)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, moveSpeed);

                if (Vector3.Distance(transform.position, targetPosition) < 1.0f)
                {
                    isCharging = false;
                    previousRand = targetPosition;
                    timeBetweenMovementsTimer = timeBetweenMovements;
                }
            }
            else
            {
                if (timeBetweenMovementsTimer > 0)
                {
                    timeBetweenMovementsTimer -= Time.deltaTime;
                }
                else
                {
                    Vector3 randomVector = Random.insideUnitCircle.normalized;
                    Debug.Log(randomVector.magnitude);
                    targetPosition = transform.position + randomVector * moveDistance;
                    isCharging = true;
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed * Time.deltaTime);
        }
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
