using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : AI
{

    [Header("Melee Variables")]
    public bool isMovingRandomly;
    public bool isCharging;
    public Vector3 targetPosition;
    public float distanceNear;
    public Vector3 velocity;
    public Vector3 previousRand;
    public float timeBetweenMovements;
    public float timeBetweenMovementsTimer;
    public float movementDistance;
    public GameObject meleeGameObject;
    public LayerMask walls;



    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BasicAttack();
        }
    }

    public override void BasicAttack()
    {
        //do ranged attack
        if (basicAttackTimer > 0)
        {
            basicAttackTimer -= Time.deltaTime;
        }
        else
        {
            Player pl = playerReference.GetComponent<Player>();
            if (pl)
            {
                pl.TakeDamage(basicAttackDamage);
                basicAttackTimer = basicAttackCooldown;
            }
        }
    }

    public override void PlayerBasicAttack(Vector3 direction)
    {
        //do ranged attack
        if (basicAttackTimer > 0)
        {
            basicAttackTimer -= Time.deltaTime;
        }
        else
        {
            GameObject go = Instantiate(meleeGameObject, transform.position, Quaternion.identity);
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);

            basicAttackTimer = basicAttackCooldown;
        }
    }

    public override void AbilityOne()
    {

    }

    public override void Movement()
    {
        if (Vector3.Distance(transform.position, playerReference.transform.position) < distanceNear)
        {
            isMovingRandomly = false;
        }

        if (isMovingRandomly)
        {
            if (isCharging)
            {
                MoveTowards(targetPosition, movementSpeed);

                if (Vector3.Distance(transform.position, targetPosition) < 1.0f)
                {
                    Debug.Log("reached Pos");
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

                    RaycastHit2D hit = Physics2D.Raycast(rb2D.position, randomVector.normalized, movementDistance, walls);
                    Debug.DrawRay(rb2D.position, randomVector.normalized * movementDistance);
                    if (hit.collider != null)
                    {
                        targetPosition = hit.point - (Vector2)randomVector.normalized * (hitbox.bounds.extents.x / 2);
                    }
                    else
                    {
                        targetPosition = transform.position + randomVector.normalized * movementDistance;
                    }
                    isCharging = true;
                }
            }
        }
        else
        {
            MoveTowards(playerReference.transform.position, movementSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}
