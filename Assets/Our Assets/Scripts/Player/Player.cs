﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region stats
    //-----------------------
    //The set up for the player
    //-----------------------
    #region player set up in editor
    [Header("Health")]
    public int baseMaxHealth;

    public int baseHealth;

    [Header("Movement")]
    public float baseMoveSpeed;

    [Range(0, 1)]
    public int baseCanFly;

    [Header("Projectiles")]
    public int baseDamage;

    public float baseFastestSecondsPerShot;

    public float baseSecondsPerShot;

    public float baseProjectileSpeed;

    public float baseProjectileRange;

    public TempProjectile baseProjectile;

    [Header("Charged Shot")]
    public int baseMaxCharges;

    public int baseCharges;

    public float baseChargeDamageMult;

    public float baseChargeSplashRadius;

    public float baseFastestChargeTime;

    public float baseChargeTime;

    public TempProjectile baseChargedProjectile;
    #endregion player set up in editor

    //-----------------------
    //The stored stats
    //-----------------------
    #region the stored stats
    //health
    int maxHealth;

    int health;

    //movement
    float moveSpeed;

    int canFly;

    //regular shot
    int damage;

    float fastestSecondsPerShot;

    float secondsPerShot;

    float projectileSpeed;

    float projectileRange;

    TempProjectile projectile;

    //charged shot
    int maxCharges;

    int charges;

    float chargeDamageMult;

    float chargeSplashRadius;

    float fastestChargeTime;

    float chargeTime;

    TempProjectile chargedProjectile;
    #endregion the stored stats

    //-----------------------
    //access to the stats
    //-----------------------
    #region access to the stats
    //health
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public int Health { get { return health; } set { health = value; } }

    //movement
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    public int CanFly { get { return canFly; } set { canFly = value; } }

    //shooting
    public int Damage { get { return damage; } set { damage = value; } }

    public float FastestSecondsPerShot { get { return fastestSecondsPerShot; } set { fastestSecondsPerShot = value; } }

    public float SecondsPerShot { get { return secondsPerShot; } set { secondsPerShot = value; } }

    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }

    public float ProjectileRange { get { return projectileRange; } set { projectileRange = value; } }

    public TempProjectile Projectile { get { return projectile; } set { projectile = value; } }

    //charged shot
    public int MaxCharges { get { return maxCharges; } set { maxCharges = value; } }

    public int Charges { get { return charges; } set { charges = value; } }

    public float ChargeDamageMult { get { return chargeDamageMult; } set { chargeDamageMult = value; } }

    public float ChargeSplashRadius { get { return chargeSplashRadius; } set { chargeSplashRadius = value; } }

    public float FastestChargeTime { get { return fastestChargeTime; } set { fastestChargeTime = value; } }

    public float ChargeTime { get { return chargeTime; } set { chargeTime = value; } }

    public TempProjectile ChargedProjectile { get { return chargedProjectile; } set { chargedProjectile = value; } }
    #endregion access to the stats
    #endregion stats

    #region Items
    public List<Item> startingItems;

    public ActiveItem heldItem;

    [HideInInspector]
    public List<Item> items = new List<Item>();

    #endregion Items

    #region Movement
    [Tooltip("The movement joystick for mobile controls")]
    public Joystick movementControl;

    public bool stickAcceleration = true;

    [Range(0, 1)]
    public float slowMult = 0.8f;

    private Rigidbody2D rb2D;

    private Vector2 velocity;

    //velocity last frame
    private Vector2 lastVelocity;

    #endregion Movement

    #region Shooting
    [Tooltip("The shooting joystick for mobile controls")]
    public Joystick shootingControl;

    public bool smoothedShooting = true;

    public Button chargedShotButton;

    float shotTimer = 0;

    float chargedShotTimer = 0;

    bool wasShooting = false;

    bool isCharged = false;

    bool isCharging = false;

    public Image chargeBar;

    Vector2 lastshootDir = Vector2.zero;

    #endregion Shooting

    [Header("Invincibility")]
    public float invincibleTimeAmount;
    private bool invincible = false;
    float invincibleTimer;
    public bool takeDamage = false;
    public float timeBetweenBlinks;
    float blinkTimer = 0.0f;

    private void Awake()
    {
        #region applying stats 
        //health
        maxHealth = baseMaxHealth;
        health = baseHealth;

        if (health > maxHealth) health = maxHealth;

        //movement
        moveSpeed = baseMoveSpeed;
        canFly = baseCanFly;

        //regular shot
        damage = baseDamage;
        fastestSecondsPerShot = baseFastestSecondsPerShot;
        secondsPerShot = baseSecondsPerShot;

        if (secondsPerShot < fastestSecondsPerShot) secondsPerShot = fastestSecondsPerShot;

        projectileSpeed = baseProjectileSpeed;
        projectileRange = baseProjectileRange;

        projectile = baseProjectile;

        //charged shot
        maxCharges = baseMaxCharges;
        charges = baseCharges;

        if (charges > maxCharges) charges = maxCharges;

        chargeDamageMult = baseChargeDamageMult;
        chargeSplashRadius = baseChargeSplashRadius;

        fastestChargeTime = baseFastestChargeTime;
        chargeTime = baseChargeTime;

        chargedProjectile = baseChargedProjectile;
        #endregion applying stats

        #region Filling out Items

        foreach (Item item in startingItems) { items.Add(item); }
        #endregion Filling out Items

        DontDestroyOnLoad(gameObject);
        rb2D = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }

    private void Start()
    {
        //get things in the scene
        VariableJoystick[] variableJoysticks = FindObjectsOfType<VariableJoystick>();
        for (int i = 0; i < variableJoysticks.Length; i++)
        {        
            if (variableJoysticks[i].CompareTag("MoveStick"))
                movementControl = variableJoysticks[i];
            if (variableJoysticks[i].CompareTag("ShootStick"))
                shootingControl = variableJoysticks[i];
        }
    }

    #region damage handling
    public void TakeDamage(int _damage)
    {
        takeDamage = false;
        if (invincible) return;
        health -= _damage;
        invincible = true;
        if (health <= 0) Die();
    }

    private void Die()
    {
        
    }

    public void Blink()
    {
        blinkTimer -= Time.deltaTime;
        if (blinkTimer <= 0)
        {
            Renderer[] things = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in things)
            {
                r.enabled = !r.enabled;
                blinkTimer = timeBetweenBlinks;
            }
        }
    }
    #endregion damage handling

    private void Update()
    {
        #region movement
        Vector2 frameVel = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) { frameVel.y += 1; }
        if (Input.GetKey(KeyCode.S)) { frameVel.y -= 1; }
        if (Input.GetKey(KeyCode.D)) { frameVel.x += 1; }
        if (Input.GetKey(KeyCode.A)) { frameVel.x -= 1; }

        frameVel.Normalize();

        if (movementControl != null)
        {
            if (movementControl.Direction.sqrMagnitude > 0) { frameVel = movementControl.Direction; }
            if (!stickAcceleration) frameVel.Normalize();
        }

        velocity = frameVel * moveSpeed;

        //Smoothes the player when they stop moving so it's not so jerky
        if ((lastVelocity.sqrMagnitude > velocity.sqrMagnitude) && lastVelocity.sqrMagnitude > 0.5)
        {
            velocity = (lastVelocity * slowMult);
        }
        //do stuff with last velocity mayhaps?
        lastVelocity = velocity;
        #endregion movement

        #region shooting

        Vector2 shootDir = Vector2.zero;
        bool isShooting = false;
        bool releasedKey = false;

        #region pc shooting controls
        if (Input.GetKeyUp(KeyCode.UpArrow))    { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.DownArrow))  { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.LeftArrow))  { releasedKey = true; }

        if (Input.GetKey(KeyCode.UpArrow))      { shootDir.y += 1; isShooting = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.DownArrow))    { shootDir.y -= 1; isShooting = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.RightArrow))   { shootDir.x += 1; isShooting = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.LeftArrow))    { shootDir.x -= 1; isShooting = true; releasedKey = false; }
        #endregion pc shooting controls

        #region mobile shooting controls
        if (movementControl != null)
        {
            if (smoothedShooting)
            {
                if (shootingControl.Direction.sqrMagnitude > 0) { shootDir = shootingControl.Direction; isShooting = true; }
            }
            else
            {
                if (shootingControl.Horizontal > 0.1f) { shootDir.x += 1; isShooting = true; }
                if (shootingControl.Horizontal < -0.1f) { shootDir.x -= 1; isShooting = true; }
                if (shootingControl.Vertical > 0.1f) { shootDir.y += 1; isShooting = true; }
                if (shootingControl.Vertical < -0.1f) { shootDir.y -= 1; isShooting = true; }
            }
            //if (movementControl.helddowntime > Time.deltaTime) { isCharging = true; }
            //else { isCharging = false; }
        }
        #endregion mobile shooting controls

        //Based on inputs shoot a projectile in the intended direction
        if (isShooting && shotTimer <= Time.time && (!Input.GetKey(KeyCode.Space) /*|| !isCharging */))
        {
            if (projectile != null)
            {
                TempProjectile p;
                p = Instantiate(projectile, rb2D.position, Quaternion.identity);
                p.transform.Rotate(new Vector3(0, 0, 1), (180 * Mathf.Atan2(shootDir.y, shootDir.x)) / Mathf.PI - 90);
                p.speed = projectileSpeed + velocity.magnitude;
                p.damageAmount = damage;
                p.lifeTime = projectileRange;
            }
            shotTimer = secondsPerShot + Time.time;
        }

        if ((Input.GetKey(KeyCode.Space) /*||isCharging*/) && charges > 0 && !isCharged)
        {
            if (chargedShotTimer >= chargeTime)
            {
                if (chargedProjectile != null)
                {
                    isCharged = true;
                }
            }
            else
            {
                chargedShotTimer += Time.deltaTime;
            }
            chargeBar.fillAmount = chargedShotTimer / chargeTime;

            if (chargeBar.fillAmount == 1)
                chargeBar.color = new Color(1, 0, 0.75f);
            else
                chargeBar.color = Color.white;
        }
        else if (isCharged && (releasedKey || (wasShooting && !isShooting)))
        {
            TempProjectile p;
            p = Instantiate(chargedProjectile, rb2D.position, Quaternion.identity);
            p.transform.Rotate(new Vector3(0, 0, 1), (180 * Mathf.Atan2(lastshootDir.y, lastshootDir.x)) / Mathf.PI - 90);
            p.speed = projectileSpeed * 1.1f;
            p.damageAmount = damage;
            p.lifeTime = projectileRange;
            chargeBar.color = Color.white;
            isCharged = false;
            chargedShotTimer = 0;
            chargeBar.fillAmount = chargedShotTimer / chargeTime;
            charges--;
        }
        else if (chargedShotTimer > 0 && !Input.GetKey(KeyCode.Space))
        {
            chargeBar.fillAmount = chargedShotTimer / chargeTime;
            if (chargeBar.fillAmount == 1) { chargeBar.color = new Color(1, 0, 0.75f); }
            else { chargeBar.color = Color.white; }
            chargedShotTimer -= Time.deltaTime * 4;
            isCharged = false;
            if (chargedShotTimer < 0) chargedShotTimer = 0;
        }
        wasShooting = isShooting;
        lastshootDir = shootDir;
        #endregion shooting

        #region invincible
        if (takeDamage) TakeDamage(1);

        if (invincible)
        {
            Blink();
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                invincible = false;
                invincibleTimer = invincibleTimeAmount;
                Renderer[] things = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in things)
                {
                    r.enabled = true;
                }
            }
        }
        #endregion invincible
    }

    private void FixedUpdate()
    {
        //rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
        rb2D.velocity = velocity * Time.fixedDeltaTime * 100;
    }
}
