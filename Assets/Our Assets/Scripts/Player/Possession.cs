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
        Health = possessed.currentHealth;
        possessionTimer = Time.time + possesser.possessionTime;
        possesser.enabled = false;
    }

    protected override void Update()
    {
        if(possessionTimer < Time.time || Input.GetKeyDown(KeyCode.LeftShift))
        {
            UnPossess();
            return;
        }
        UpdateMovement();

        InvincibilityChecks();
    }

    protected override void Die()
    {
        UnPossess();
    }

    private void UnPossess()
    {
        possesser.enabled = true;
        Destroy(this);
    }

}
