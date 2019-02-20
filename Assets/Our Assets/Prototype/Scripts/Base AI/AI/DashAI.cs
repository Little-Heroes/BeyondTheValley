using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAI : AI
{
    [Header("Dash Values")]
    public float dashSpeed;
    public Vector3 dashPoint;
    public bool isCharging;

    public Vector3 beginPos;

    // Update is called once per frame
    void Update()
    {
        BasicAttack();
    }

    public override void BasicAttack()
    {
        if (abilityOneTimer > 0)
        {
            abilityOneTimer -= Time.deltaTime;
        }
        else
        {
            if (isCharging)
            {
                MoveTowards(dashPoint, movementSpeed);

                if (Vector3.Distance(rb2D.position, dashPoint) < 1.0f)
                {
                    isCharging = false;
                    abilityOneTimer = abilityOneCooldown;
                }
            }
            else
            {
                beginPos = rb2D.position;
                dashPoint = playerReference.transform.position;
                isCharging = true;
            }
        }
    }

    public override void PlayerAbilityOne(Vector3 direction)
    {
        base.PlayerAbilityOne(direction);
    }
}
