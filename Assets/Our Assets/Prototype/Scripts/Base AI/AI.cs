using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    public int contactDamage;

    public float movementSpeed;



	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
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
    }
}
