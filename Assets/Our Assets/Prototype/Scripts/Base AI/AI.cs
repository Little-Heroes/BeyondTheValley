using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    public enum Teams
    {
        Player,
        Enemy, 
        Party, 
        EnemySquared,
    }

    [Header("Base AI Variables")]
    public string AItype;
    public float bonusPossessionTime;
    public int maxHealth;
    public int currentHealth;
    public float movementSpeed;
    public bool isAwake;
    public GameObject playerReference;
    public Rigidbody2D rb2D;
    public Teams startingTeam;
    public Teams currentTeam;
    public bool possessed = false;
    public bool isStunned = false;

    [Header("Attack Variables")]
    public int basicAttackDamage;
    public float basicAttackCooldown;
    public float basicAttackTimer;

    public int contactDamage;

    public int abilityOneDamage;
    public float abilityOneCooldown;
    public float abilityOneTimer;


    // Use this for initialization
    void Start ()
    {
        currentHealth = maxHealth;
        playerReference = GameObject.FindGameObjectWithTag("Player");
        rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    
    public virtual void DealContactDamage(Player player)
    {
        player.TakeDamage(contactDamage);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
        Possession po = GameObject.FindObjectOfType<Possession>();
        //po.OnKill(AItype, bonusPossessionTime);
    }

    public virtual void Movement()
    {
        Vector3 dir = playerReference.GetComponent<Rigidbody2D>().position - rb2D.position;
        rb2D.MovePosition(rb2D.position + (Vector2)dir.normalized * movementSpeed * Time.deltaTime);
    }

    public virtual void BasicAttack()
    {
        if(basicAttackTimer > 0)
        {
            basicAttackTimer -= Time.deltaTime;
        }
        else
        {
            //
        }
    }

    public virtual void AbilityOne()
    {
        if (abilityOneTimer > 0)
        {
            abilityOneTimer -= abilityOneCooldown;
        }
        else
        {
            //
        }
    }

    public virtual void PlayerBasicAttack(Vector3 direction)
    {

    }

    public virtual void PlayerAbilityOne(Vector3 direction)
    {

    }

    public virtual void MoveTowards(Vector2 target, float movementSpeed)
    {
        Vector2 direction = target - rb2D.position;
        rb2D.MovePosition(rb2D.position + direction.normalized * movementSpeed * Time.deltaTime);
    }
}
