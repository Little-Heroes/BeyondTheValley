﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAI : AI
{
    [Header("Dash Values")]
    public float maxDashSpeed;
    public float dashSpeed;
    public Vector3 dashPoint;
    public bool isCharging;
    public AnimationCurve animCurve;
    public float timeElapsed = 0.0f;
    public float playerDashAmount;

    public Vector3 beginPos;

    // Update is called once per frame
    protected override void Update()
    {
        if (isStunned)
            return;
        BasicAttack();
        base.Update();
    }

    public override void BasicAttack()
    {
        if (basicAttackTimer > 0)
        {
            basicAttackTimer -= Time.deltaTime;
        }
        else
        {
            if (isCharging)
            {
                float distanceTravelled = Vector3.Distance(beginPos, rb2D.position);
                float startToEnd = Vector3.Distance(beginPos, dashPoint);
                float distance = distanceTravelled / startToEnd;
                dashSpeed = animCurve.Evaluate(distance) * maxDashSpeed;
                Debug.Log(distance);
                MoveTowards(dashPoint, dashSpeed);

                if (Vector3.Distance(rb2D.position, dashPoint) < 1.0f)
                {
                    isCharging = false;
                    basicAttackTimer = basicAttackCooldown;
                    dashSpeed = 0.0f;
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

    public override void PlayerBasicAttack(Vector3 direction)
    {
        if (basicAttackTimer > 0)
        {
            basicAttackTimer -= Time.deltaTime;
        }
        else
        {
            if (isCharging)
            {
                //todo: disable player movement
                playerReference.GetComponent<Player>().canMove = false;

                float distanceTravelled = Vector3.Distance(beginPos, rb2D.position);
                float startToEnd = Vector3.Distance(beginPos, dashPoint);
                float distance = distanceTravelled / startToEnd;
                dashSpeed = animCurve.Evaluate(distance) * maxDashSpeed;
                Debug.Log(distance);
                MoveTowards(dashPoint, dashSpeed);

                if (Vector3.Distance(rb2D.position, dashPoint) < 1.0f)
                {
                    //todo: re-enable player movement
                    playerReference.GetComponent<Player>().canMove = true;
                    isCharging = false;
                    basicAttackTimer = basicAttackCooldown;
                    dashSpeed = 0.0f;
                }
            }
            else
            {
                beginPos = rb2D.position;
                dashPoint = rb2D.position + (Vector2)direction * playerDashAmount;
                isCharging = true;
            }
        }
    }

    public override void PlayerAbilityOne(Vector3 direction)
    {
        base.PlayerAbilityOne(direction);
    }

    public override void OnPossession()
    {
        isCharging = false;
        base.OnPossession();
    }
}
