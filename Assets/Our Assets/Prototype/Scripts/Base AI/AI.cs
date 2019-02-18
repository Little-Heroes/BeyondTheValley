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

    public int maxHealth;
    public int currentHealth;
    public int contactDamage;
    public float movementSpeed;
    public bool isAwake;
    public GameObject playerReference;
    public Rigidbody2D rb2D;
    public Teams startingTeam;
    public Teams currentTeam;



	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
        playerReference = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 dir = playerReference.GetComponent<Rigidbody2D>().position - rb2D.position;
        //rb2D.MovePosition(rb2D.position + (Vector2)dir.normalized * movementSpeed * Time.deltaTime);
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
    }

    public virtual void Movement()
    {
        Vector3 dir = playerReference.GetComponent<Rigidbody2D>().position - rb2D.position;
        rb2D.MovePosition(rb2D.position + (Vector2)dir.normalized * movementSpeed * Time.deltaTime);
    }

    public virtual void BasicAttack()
    {

    }

    public virtual void AbilityOne()
    {

    }
}
