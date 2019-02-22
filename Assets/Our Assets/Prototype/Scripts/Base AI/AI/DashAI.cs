using System.Collections;
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

    private Vector2 previousPos;

    public Vector3 beginPos;
    public bool usingAbilityOne = false;
    Vector3 direction;

    bool hasReachedEnd = false;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isStunned)
            return;
        BasicAttack();
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
       
    }

    public void Dashing(Vector3 direction)
    {
        if (isCharging)
        {
            //todo: disable player movement
            gameObject.GetComponent<Possession>().canMove = false;

            float distanceTravelled = Vector3.Distance(beginPos, rb2D.position);
            float startToEnd = Vector3.Distance(beginPos, dashPoint);
            float distance = distanceTravelled / startToEnd;
            dashSpeed = animCurve.Evaluate(distance) * maxDashSpeed;
            MoveTowards(dashPoint, dashSpeed);

            if (Vector3.Distance(rb2D.position, dashPoint) < 1.0f)
            {
                //todo: re-enable player movement
                gameObject.GetComponent<Possession>().canMove = true;
                isCharging = false;
                basicAttackTimer = basicAttackCooldown;
                dashSpeed = 0.0f;
                usingAbilityOne = false;
            }
            if (previousPos == rb2D.position)
            {
                //todo: re-enable player movement
                gameObject.GetComponent<Possession>().canMove = true;
                isCharging = false;
                basicAttackTimer = basicAttackCooldown;
                dashSpeed = 0.0f;
                usingAbilityOne = false;
            }
            previousPos = rb2D.position;
        }
        else
        {
            beginPos = rb2D.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, playerDashAmount);
            if (hit.collider != null)
                dashPoint = rb2D.position + (Vector2)direction * (hit.distance - hitbox.bounds.extents.x / 2);
            else
                dashPoint = rb2D.position + (Vector2)direction * playerDashAmount;
            isCharging = true;
        }
    }

    public override void PlayerAbilityOne(Vector3 direction)
    {
        this.direction = direction;
        hasReachedEnd = false;
        StartCoroutine(Dash());
    }

    public override void OnPossession()
    {
        isCharging = false;
        base.OnPossession();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(dashPoint, 0.5f);
    }

    public IEnumerator Dash()
    {
        while (!hasReachedEnd)
        {
            if (isCharging)
            {
                //todo: disable player movement
                gameObject.GetComponent<Possession>().canMove = false;

                float distanceTravelled = Vector3.Distance(beginPos, rb2D.position);
                float startToEnd = Vector3.Distance(beginPos, dashPoint);
                float distance = distanceTravelled / startToEnd;
                dashSpeed = animCurve.Evaluate(distance) * maxDashSpeed;
                MoveTowards(dashPoint, dashSpeed);

                if (Vector3.Distance(rb2D.position, dashPoint) < 1.0f)
                {
                    //todo: re-enable player movement
                    gameObject.GetComponent<Possession>().canMove = true;
                    isCharging = false;
                    basicAttackTimer = basicAttackCooldown;
                    dashSpeed = 0.0f;
                    usingAbilityOne = false;
                    hasReachedEnd = true;
                }
                if (previousPos == rb2D.position)
                {
                    //todo: re-enable player movement
                    gameObject.GetComponent<Possession>().canMove = true;
                    isCharging = false;
                    basicAttackTimer = basicAttackCooldown;
                    dashSpeed = 0.0f;
                    usingAbilityOne = false;
                    hasReachedEnd = true;
                }
                previousPos = rb2D.position;
            }
            else
            {
                beginPos = rb2D.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, playerDashAmount, walls);
                if (hit.collider != null)
                    dashPoint = rb2D.position + (Vector2)direction * (hit.distance - hitbox.bounds.extents.x / 2);
                else
                    dashPoint = rb2D.position + (Vector2)direction * playerDashAmount;
                isCharging = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
