using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    [Header("Boss Health Variables")]
    public int maxHealth;
    public int currentHealth;
    public int shields;
    public float damageMultiplier;
    public Image healthBar;

    [Header("Boss Stun Variables")]
    public bool isStunned = false;
    public float maxResistance = 2.0f;
    public float stunLimit = 10f;
    public float resistance = 2.0f;
    public float amountStunned = 0f;
    private float lastResist = 0f;
    public Image resistBar;
    public Image stunBar;
    public int stunDamage;



    // Use this for initialization
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ManageStun();
        ActuallyTakeDamage();
        
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    public virtual void BasicAttack()
    {

    }

    public virtual void AbilityOne()
    {

    }
    
    public virtual void AbilityTwo()
    {

    }

    public virtual void ActuallyTakeDamage()
    {
        if(resistance <= 0)
        {
            TakeDamage(stunDamage, false);
            isStunned = false;
            resistance = maxResistance;
            amountStunned = 0;
            OnExitStun();
        }
    }

    public virtual void ManageStun()
    {
        if (stunBar != null)
        {
            if (amountStunned <= 0) { stunBar.fillAmount = 0; }
            else { stunBar.fillAmount = amountStunned / stunLimit; }
        }
        if (resistBar != null)
        {
            if (resistance >= maxResistance) { resistBar.fillAmount = 1; }
            else { resistBar.fillAmount = resistance / maxResistance; }
        }
        //the stun cools down slowly overtime unless resistance is falling to the player
        if (resistance >= maxResistance)
        {
            amountStunned -= Time.deltaTime / 2;
        }
        //if stunned and the amount stunned is less than or equal 0 stopped being stunned
        if (isStunned)
        {
            if (amountStunned <= 0)
            {
                isStunned = false;
                OnExitStun();
            }
        }
        //if the resistance is less than the max, increase it back to the max
        if (resistance < maxResistance)
        {
            resistance += Time.deltaTime / 2;
            if (resistance > maxResistance)
            {
                resistance = maxResistance;
            }
        }
        amountStunned = Mathf.Clamp(amountStunned, 0, stunLimit);
        lastResist = resistance;
    }

    public virtual void TakeDamage(int amount, bool stun)
    {
        if (stun)
        {
            amountStunned += amount;
            if (amountStunned >= stunLimit)
            {
                isStunned = true;
                OnStun();
            }
        }
        else
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        //do death stuff
    }

    public virtual void OnStun()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public virtual void OnExitStun()
    {
        GetComponent<Collider2D>().isTrigger = false;
    }
}
