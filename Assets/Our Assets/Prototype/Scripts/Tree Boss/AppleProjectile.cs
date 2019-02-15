using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleProjectile : MonoBehaviour
{

    public float moveSpeed;
    public float frequency;
    public float magnitude;
    public Vector3 direction;

    public bool fired = true;
    public float appleRadius;

    public float deadTime;
    float deadTimeCounter;

    // Use this for initialization
    void Start()
    {
        deadTimeCounter = deadTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            transform.position = transform.position + (transform.right * Mathf.Sin(Time.time * frequency) * magnitude) + transform.up * moveSpeed * Time.deltaTime;
            deadTimeCounter -= Time.deltaTime;
            if (deadTimeCounter <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void DealDamage()
    {
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, appleRadius);
        if (overlap)
        {
            Player player = overlap.gameObject.GetComponent<Player>();
            if (player)
            {
                player.TakeDamage(1);
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
