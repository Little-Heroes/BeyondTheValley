using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public float health;
    public float chargeCooldown;
    float chargeTimer;

    public float timeBetweenMovements;
    float timeBetweenMovementsTimer;

    public float chargeDamageAmount;

    public float chargeDistance;

    public float chargeSpeed;

    Vector3 targetPosition;
    GameObject player;

    bool isCharging;
    bool isMoving;

    Vector3 chargePoint;

    Vector3 startPosition;
    float chargeDist;

    public float contactDamageCooldown;
    float contactDamagetimer;

    Vector3 velocity = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        chargeTimer = chargeCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging)
        {

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, chargeSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 1.0f)
            {
                isCharging = false;
                chargeTimer = chargeCooldown;
            }
        }
        else
        {
            if (chargeTimer > 0)
            {
                chargeTimer -= Time.deltaTime;
            }
            else
            {
                startPosition = transform.position;
                targetPosition = player.transform.position;
                isCharging = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging)
        {
            GameObject go = collision.gameObject;
            if (go.tag == "Player")
            {
                Player tpc = go.GetComponent<Player>();
                tpc.TakeDamage((int)chargeDamageAmount);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collGO = collision.gameObject;
        if (collGO.tag == "Player")
        {
            if (contactDamagetimer <= 0)
            {
                collGO.GetComponent<Player>().TakeDamage((int)chargeDamageAmount);
                contactDamagetimer = contactDamageCooldown;
            }
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
