using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBomb : MonoBehaviour
{

    public bool hasLanded;
    public float appleRadius;
    public float speed;

    public float bombCountdown;
    public float bombCountdownTimer;
    public bool countingDown = false;
    public bool thrown = false;
    public Vector3 endPos;
    public GameObject shadow;
    public GameObject warnCirc;
    public float moveSpeed;

    // Use this for initialization
    void Start()
    {
        bombCountdownTimer = bombCountdown;
        if (thrown)
            warnCirc = Instantiate(shadow, endPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (thrown)
        {

            transform.position = Vector3.Lerp(transform.position, endPos, moveSpeed * Time.deltaTime);

            if (warnCirc)
                warnCirc.transform.localScale = Vector3.Lerp(warnCirc.transform.localScale, Vector3.zero, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, endPos) < 1.0f)
            {
                StartCountdown();
                thrown = false;
            }
        }
        if (countingDown)
        {
            bombCountdownTimer -= Time.deltaTime;
            if (bombCountdownTimer <= 0)
            {
                //blow up
                //do an animation
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
                if (warnCirc)
                    Destroy(warnCirc);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, appleRadius);
    }

    public void SetHasLanded()
    {
        hasLanded = true;
    }

    public void StartCountdown(float time)
    {
        countingDown = true;
        bombCountdownTimer = time;
    }
    public void StartCountdown()
    {
        countingDown = true;
        bombCountdownTimer = bombCountdown;
    }
}
