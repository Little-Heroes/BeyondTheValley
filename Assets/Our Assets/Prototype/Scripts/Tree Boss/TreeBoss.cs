using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBoss : Boss
{
    [Header("Rain fire(apples) ability")]
    public GameObject appleBomb;
    public GameObject appleProjectile;
    public int numberOfApples;
    public float rainApplesCooldown;
    float rainApplesTimer;
    public Transform[] appleSpawns;
    [Tooltip("0 to 100")]
    public int bombChance;

    [Header("Shooty boy")]
    public float shotCooldown;
    float shotCooldownTimer;
    GameObject player;

    [Header("Contact bomb drop")]
    public float bombDropCooldown;
    float bombDropTimer;

    public int numberOfBombs;
    public int circleRadius;
    bool droppingBombs = false;

    public Transform bossParentTransform;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        bombDropTimer = bombDropCooldown;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isStunned)
            return;

        if (shotCooldownTimer > 0)
        {
            shotCooldownTimer -= Time.deltaTime;
        }
        else
        {
            Vector3 heading = player.transform.position - transform.position;
            float mag = heading.magnitude;
            Vector3 normalized = heading / mag;
            GameObject go = Instantiate(appleProjectile, transform.position, Quaternion.identity);
            go.transform.parent = bossParentTransform;
            float rotZ = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);
            Destroy(go.GetComponent<ThrownApple>());
            shotCooldownTimer = shotCooldown;
        }

        if (rainApplesTimer > 0)
        {
            rainApplesTimer -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < numberOfApples; i++)
            {
                int rng = Random.Range(0, 101);
                GameObject go;
                if (rng < bombChance)
                {
                    go = Instantiate(appleBomb, appleSpawns[i].position, Quaternion.identity);
                    go.GetComponent<AppleBomb>().thrown = false;
                    go.transform.parent = bossParentTransform;
                }
                else
                {
                    go = Instantiate(appleProjectile, appleSpawns[i].position, Quaternion.identity);
                    go.GetComponent<AppleProjectile>().fired = false;
                    go.transform.parent = bossParentTransform;
                }
                go.gameObject.layer = LayerMask.NameToLayer("NotCollideWithPlayer");
            }
            rainApplesTimer = rainApplesCooldown;
        }

        if (droppingBombs)
        {
            rainApplesTimer = rainApplesCooldown;
            if (bombDropTimer > 0)
            {
                bombDropTimer -= Time.deltaTime;
            }
            else
            {
                for (int i = 0; i < numberOfBombs; i++)
                {
                    float j = ((float)(i * 1) / numberOfBombs);
                    float angle = j * (Mathf.PI) * 2.0f;
                    float x = Mathf.Sin(angle) * circleRadius;
                    float y = Mathf.Cos(angle) * circleRadius;
                    Vector3 pos = new Vector3(x, y, 0) + transform.position;
                    GameObject go = Instantiate(appleBomb, transform.position, Quaternion.identity);
                    go.transform.parent = bossParentTransform;
                    Destroy(go.GetComponent<ThrownApple>());
                    go.GetComponent<AppleBomb>().endPos = pos;
                    go.GetComponent<AppleBomb>().thrown = true;
                }
                bombDropTimer = bombDropCooldown;
                droppingBombs = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!droppingBombs)
            droppingBombs = true;
    }
}
