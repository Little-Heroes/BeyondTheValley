using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : AI {

    [Header("Ranged Enemy Elements")]
    public GameObject projectile;
    public float minimumRangeDistance;
    public bool inRange;
    public float imTooCloseDistance;
    public int damage;
    
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        if (isStunned)
        {
            Debug.Log(rb2D.velocity);
            return;
        }
        Movement();
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
            Vector3 heading = playerReference.transform.position - transform.position;
            float mag = heading.magnitude;
            Vector3 normalized = heading / mag;
            GameObject go = Instantiate(projectile, transform.position, Quaternion.identity);
            float rotZ = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);

            go.layer = LayerMask.NameToLayer("EnemyProjectile");
            basicAttackTimer = basicAttackCooldown;
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
            GameObject go = Instantiate(projectile, transform.position, Quaternion.identity);
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ - 90);
            go.GetComponent<TempProjectile>().damageAmount = basicAttackDamage;
            

            go.layer = LayerMask.NameToLayer("PlayerProjectile");
            basicAttackTimer = basicAttackCooldown;
        }
    }

    public override void Movement()
    {
        float distance = Vector3.Distance(transform.position, playerReference.transform.position);
        if (distance > minimumRangeDistance)
        {
            inRange = false;
            MoveTowards(playerReference.transform.position, movementSpeed);
        }
        else if (distance <= minimumRangeDistance && distance > imTooCloseDistance)
        {
            BasicAttack();
        }
        else
        {
            //move away
            Vector3 heading = playerReference.transform.position - transform.position;
            float mag = heading.magnitude;
            Vector3 normalized = heading / mag;
            
            MoveTowards(transform.position - normalized, movementSpeed);
        }
    }
}
