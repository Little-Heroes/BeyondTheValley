using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public enum SpawnType
    {
        InBounds,
        SpawnPoints
    }

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
    public bool canBeStunned = true;
    public float stunCooldown;
    public float stunCooldownTimer;

    [Header("Spawn Minions")]
    public GameObject[] minionsToSpawn;
    public GameObject[] spawnPoints;
    public Collider2D spawnBounds;
    public SpawnType spawnType;
    public float spawnMinionCD;
    public float spawnMinionTimer;
    [Range(0, 100)]
    public float chanceOfSpawningEnemy;
    bool canSpawnHere = false;
    int index = 0;


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
        SpawnEnemies();

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
        if (resistance <= 0)
        {
            canBeStunned = false;
            TakeDamage(stunDamage, false);
            isStunned = false;
            resistance = maxResistance;
            amountStunned = 0;
            OnExitStun();
        }
    }

    public virtual void ManageStun()
    {
        if(!canBeStunned)
        {
            stunCooldownTimer -= Time.deltaTime;
            if(stunCooldownTimer <= 0)
            {
                stunCooldownTimer = stunCooldown;
                canBeStunned = true;
            }
        }
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
        if (stun && canBeStunned)
        {
            amountStunned += amount;
            if (amountStunned >= stunLimit)
            {
                isStunned = true;
                OnStun();
            }
        }
        else if(!stun)
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

    public virtual void SpawnEnemies()
    {
        if (isStunned)
            return;
        if (spawnMinionTimer > 0)
        {
            spawnMinionTimer -= Time.deltaTime;
        }
        else
        {
            int rng = Random.Range(0, 101);
            if (rng < chanceOfSpawningEnemy)
            {
                switch (spawnType)
                {
                    case SpawnType.InBounds:
                        Vector3 randomPosition = new Vector3();
                        while (!canSpawnHere)
                        {
                            randomPosition = new Vector3(Random.Range(spawnBounds.bounds.min.x, spawnBounds.bounds.max.x), Random.Range(spawnBounds.bounds.min.y, spawnBounds.bounds.max.y), 0);

                            if(index > 100)
                            {

                            }

                            Collider2D hit = Physics2D.OverlapCircle(randomPosition, spawnBounds.bounds.size.magnitude);
                            if (hit.gameObject.tag == "Boss")
                            {
                                
                            }
                            else
                            {
                                canSpawnHere = true;
                            }
                            index++;
                        }

                        int randomMinion = Random.Range(0, minionsToSpawn.Length);
                        Instantiate(minionsToSpawn[randomMinion], randomPosition, Quaternion.identity);

                        break;
                    case SpawnType.SpawnPoints:

                        break;
                }
            }
            spawnMinionTimer = spawnMinionCD;
        }
    }
}
