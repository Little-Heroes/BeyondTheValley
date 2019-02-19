using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : Player {

    public Player possesser;
    public AI possessed;

    float possessionTimer = 0f;

    protected override void Start()
    {
        possessed = GetComponent<AI>();
        MoveSpeed = possessed.movementSpeed;
        MaxHealth = possessed.maxHealth;
        Health = possessed.maxHealth;
        possessionTimer = Time.time + possesser.possessionTime;
        possesser.enabled = false;
    }

    protected override void UpdateAttacking()
    {
        Vector2 attackDir = Vector2.zero;
        bool isAttacking = false;
        bool releasedKey = false;

        #region pc attacking inputs
        if (Input.GetKeyUp(KeyCode.UpArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { releasedKey = true; }

        if (Input.GetKey(KeyCode.UpArrow)) { attackDir.y += 1; isAttacking = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.DownArrow)) { attackDir.y -= 1; isAttacking = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.RightArrow)) { attackDir.x += 1; isAttacking = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.LeftArrow)) { attackDir.x -= 1; isAttacking = true; releasedKey = false; }
        #endregion pc attacking inputs
        //Based on inputs attack in the intended direction
        if (isAttacking && attackTimer <= Time.time && (!Input.GetKey(KeyCode.Space)))
        {
            //possessed.PlayerBasicAttack(attackDir);
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            if (chargedAttackTimer >= ChargeTime)
            {
                isCharged = true;
            }
            else
            {
                chargedAttackTimer += Time.deltaTime;
            }
            chargeBar.fillAmount = chargedAttackTimer / ChargeTime;

            if (chargeBar.fillAmount == 1)
                chargeBar.color = new Color(1, 0, 0.75f);
            else
                chargeBar.color = Color.white;
        }
        else if (isCharged && (releasedKey || (wasAttacking && !isAttacking)))
        {
            //possessed.PlayerChargedAttack(attackDir);
            chargeBar.color = Color.white;
            isCharged = false;
            chargedAttackTimer = 0;
            chargeBar.fillAmount = chargedAttackTimer / ChargeTime;
        }
        else if (chargedAttackTimer > 0 && !Input.GetKey(KeyCode.Space))
        {
            chargeBar.fillAmount = chargedAttackTimer / ChargeTime;
            if (chargeBar.fillAmount == 1) { chargeBar.color = new Color(1, 0, 0.75f); }
            else { chargeBar.color = Color.white; }
            chargedAttackTimer -= Time.deltaTime * 4;
            isCharged = false;
            if (chargedAttackTimer < 0) chargedAttackTimer = 0;
        }
        wasAttacking = isAttacking;
        lastAttackDir = attackDir;
    }

    protected override void Update()
    {
        if(possessionTimer < Time.time)
        {
            Expunge();
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            StunRing();
            Expunge();
            return;
        }
        base.Update();
    }

    protected override void Die()
    {
        Expunge();
    }

    public void OnKill(string enemyType, float possessTimePlus)
    {
        if(/*possessed.AIType == enemyType*/false)
        {
            possessionTimer += possessTimePlus * 2;
        }
        possessionTimer += possessTimePlus;
    }

    private void StunRing()
    {
        //do the particle display
        //stun all in the radius
        //knock back those just outside the radius
    }

    private void Expunge()
    {
        //possessed.animator.SetBool("UnPossess", true);
        possesser.enabled = true;
        Destroy(this);
    }
}
