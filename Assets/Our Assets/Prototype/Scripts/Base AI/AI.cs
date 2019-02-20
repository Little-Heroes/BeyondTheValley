using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
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
    //possession stuff
    public bool possessed = false;
    public bool isStunned = false;
    public float maxResistance = 2.0f;
    public float stunLimit = 10f;
    public float resistance = 2.0f;
    public float amountStunned = 0f;
    private float lastResist = 0f;
    public Image resistBar;
    public Image stunBar;
    public Collider2D hitbox;

    [Header("Attack Variables")]
    public int basicAttackDamage;
    public float basicAttackCooldown;
    public float basicAttackTimer;

    public int contactDamage;

    public int abilityOneDamage;
    public float abilityOneCooldown;
    public float abilityOneTimer;

    [Header("debug stuff")]
    public Vector3 gizmoPoint;


    // Use this for initialization
    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        currentHealth = maxHealth;
        playerReference = GameObject.FindGameObjectWithTag("Player");
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
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
        if ( resistance >= maxResistance )
        {
            amountStunned -= Time.deltaTime / 2;
        }
        //if stunned and the amount stunned is less than or equal 0 stopped being stunned
        if( isStunned )
        {
            if (amountStunned <= 0)
            {
                isStunned = false;
            }
        }
        //if the resistance is less than the max, increase it back to the max
        if( resistance < maxResistance )
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

    public virtual void DealContactDamage(Player player)
    {
        player.TakeDamage(contactDamage);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(int amount, bool stun)
    {
        if (stun)
        {
            amountStunned += amount;
            if(amountStunned >= stunLimit) { isStunned = true; }
        }
        else { TakeDamage(amount); }
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
        if (basicAttackTimer > 0)
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
        //RaycastHit2D hit = Physics2D.Raycast(rb2D.position, direction.normalized, movementSpeed, LayerMask.NameToLayer("Walls"));
        //if (hit)
        //{
        //    gizmoPoint = hit.point;
        //    rb2D.MovePosition(rb2D.position + direction.normalized * (hitbox.bounds.SqrDistance(hit.point) / 2) * Time.deltaTime);
        //}
        //else
        {
            rb2D.MovePosition(rb2D.position + direction.normalized * movementSpeed * Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gizmoPoint, 0.5f);
    }
}
