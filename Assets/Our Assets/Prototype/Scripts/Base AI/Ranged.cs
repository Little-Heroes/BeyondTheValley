using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [Header("Health variabls")]
    public float health;

    [Header("Distance variables")]
    public float minimumRangeDistance;
    public float imTooCloseDistance;

    public GameObject player;

    Vector3 velocity = Vector3.zero;
    public float moveSpeed;

    bool inRange;

    [Header("ranged attack values")]
    public GameObject projectile;
    public float rangedAttackCooldown;
    float rangedAttackTimer;
    public float rangedAttackDamage;
    public Vector3 thingy;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > minimumRangeDistance)
        {
            inRange = false;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else if (distance <= minimumRangeDistance && distance > imTooCloseDistance)
        {
            //do ranged attack
            if (rangedAttackTimer > 0)
            {
                rangedAttackTimer -= Time.deltaTime;
            }
            else
            {
                Vector3 heading = player.transform.position - transform.position;
                float mag = heading.magnitude;
                Vector3 normalized = heading / mag;
                GameObject go = Instantiate(projectile, transform.position, Quaternion.identity);
                float rotZ = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
                go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);

                go.layer = LayerMask.NameToLayer("EnemyProjectile");
                rangedAttackTimer = rangedAttackCooldown;
            }
        }
        else
        {
            //move away
            Vector3 heading = player.transform.position - transform.position;
            float mag = heading.magnitude;
            Vector3 normalized = heading / mag;

            transform.position = Vector3.MoveTowards(transform.position, transform.position - normalized, moveSpeed * Time.deltaTime);
        }

    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
