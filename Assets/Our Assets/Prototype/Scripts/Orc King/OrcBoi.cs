using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcBoi : MonoBehaviour
{

    Transform target;
    Rigidbody2D rb2D;
    public float movementSpeed;
    public float health;
    bool following = true;

    [Header("melee attack variables")]
    public float attackDamage;
    public float attackCooldown;
    float attackTimer = 0;

    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //rb2D.position = Vector3.Lerp(rb2D.position, target.position, movementSpeed * Time.time);
        if (following)
            rb2D.position = Vector3.MoveTowards(rb2D.position, target.position, movementSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            following = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            following = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collGO = collision.gameObject;
        if (collGO.tag == "Player")
        {
            if (attackTimer <= 0)
            {
                collGO.GetComponent<TempPlayerController>().TakeDamage(attackDamage);
                attackTimer = attackCooldown;
            }
        }
    }
}
