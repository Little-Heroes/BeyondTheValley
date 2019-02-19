using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Testing")]
    public bool die = false;

    public bool takeDamage = false;


    [Header("GIVE")]
    public Animator anim;


    #region stats
    //-----------------------
    //The set up for the player
    //-----------------------
    #region player set up in editor
    [Header("Stats")]
    [Header("Health")]
    public int baseMaxHealth;

    public int baseHealth;

    [Header("Movement")]
    public float baseMoveSpeed;

    [Range(0, 1)]
    public int baseCanFly;

    [Range(0, 1)]
    public float slowMult = 0.8f;

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

    public Image chargeBar;

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
    [Header("Joystick")]
    [Tooltip("The movement joystick for mobile controls")]
    public Joystick movementControl;

    public bool stickAcceleration = true;
    
    private Rigidbody2D rb2D;

    private Vector2 velocity;

    //velocity last frame
    private Vector2 lastVelocity;

    #endregion Movement

    #region Attacking
    [Tooltip("The shooting joystick for mobile controls")]
    public Joystick shootingControl;

    public bool smoothedShooting = true;

    public Button chargedAttackButton;

    protected float attackTimer = 0;

    protected float chargedAttackTimer = 0;

    protected bool wasAttacking = false;

    protected bool isCharged = false;

    protected bool isCharging = false;

    protected Vector2 lastAttackDir = Vector2.zero;

    #endregion Attacking

    #region possession
    [Header("Possession")]
    public float possessionTime;
    #endregion possession

    #region ivincibility
    [Header("Invincibility")]
    public float invincibleTimeAmount;
    private bool invincible = false;
    float invincibleTimer;
    public float timeBetweenBlinks;
    float blinkTimer = 0.0f;
    #endregion invincibility
    protected virtual void Awake()
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

        rb2D = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }

    protected virtual void Start()
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
    public virtual void TakeDamage(int _damage)
    {
        takeDamage = false;
        if (invincible) return;
        health -= _damage;
        invincible = true;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        die = false;
        if (anim != null) anim.SetBool("isDead", true);
        enabled = false;
    }

    public virtual void Blink()
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

    private void Possess()
    {

    }

    protected void UpdateMovement()
    {
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

        //play the walking animation if the player is moving
        if(anim != null)
        {
            if (velocity.sqrMagnitude > 0) anim.SetBool("isWalking", true);
            else anim.SetBool("isWalking", false);
        }

        //Smoothes the player when they stop moving so it's not so jerky
        if ((lastVelocity.sqrMagnitude > velocity.sqrMagnitude) && lastVelocity.sqrMagnitude > 0.5)
        {
            velocity = (lastVelocity * slowMult);
        }

        lastVelocity = velocity;
    }

    protected virtual void UpdateAttacking()
    {
        Vector2 attackDir = Vector2.zero;
        bool isAttacking = false;
        bool releasedKey = false;

        #region pc shooting controls
        if (Input.GetKeyUp(KeyCode.UpArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { releasedKey = true; }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { releasedKey = true; }

        if (Input.GetKey(KeyCode.UpArrow)) { attackDir.y += 1; isAttacking = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.DownArrow)) { attackDir.y -= 1; isAttacking = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.RightArrow)) { attackDir.x += 1; isAttacking = true; releasedKey = false; }
        if (Input.GetKey(KeyCode.LeftArrow)) { attackDir.x -= 1; isAttacking = true; releasedKey = false; }
        #endregion pc shooting controls

        #region mobile shooting controls
        if (movementControl != null)
        {
            if (smoothedShooting)
            {
                if (shootingControl.Direction.sqrMagnitude > 0) { attackDir = shootingControl.Direction; isAttacking = true; }
            }
            else
            {
                if (shootingControl.Horizontal > 0.1f) { attackDir.x += 1; isAttacking = true; }
                if (shootingControl.Horizontal < -0.1f) { attackDir.x -= 1; isAttacking = true; }
                if (shootingControl.Vertical > 0.1f) { attackDir.y += 1; isAttacking = true; }
                if (shootingControl.Vertical < -0.1f) { attackDir.y -= 1; isAttacking = true; }
            }
            //if (movementControl.helddowntime > Time.deltaTime) { isCharging = true; }
            //else { isCharging = false; }
        }
        #endregion mobile shooting controls

        //Based on inputs shoot a projectile in the intended direction
        if (isAttacking && attackTimer <= Time.time && (!Input.GetKey(KeyCode.Space) /*|| !isCharging */))
        {
            if (projectile != null)
            {
                TempProjectile p;
                p = Instantiate(projectile, rb2D.position, Quaternion.identity);
                p.transform.Rotate(new Vector3(0, 0, 1), (180 * Mathf.Atan2(attackDir.y, attackDir.x)) / Mathf.PI - 90);
                p.speed = projectileSpeed + velocity.magnitude;
                p.damageAmount = damage;
                p.lifeTime = projectileRange;
            }
            attackTimer = secondsPerShot + Time.time;
        }

        if ((Input.GetKey(KeyCode.Space) /*||isCharging*/) && charges > 0 && !isCharged)
        {
            if (chargedAttackTimer >= chargeTime)
            {
                if (chargedProjectile != null)
                {
                    isCharged = true;
                }
            }
            else
            {
                chargedAttackTimer += Time.deltaTime;
            }
            chargeBar.fillAmount = chargedAttackTimer / chargeTime;

            if (chargeBar.fillAmount == 1)
                chargeBar.color = new Color(1, 0, 0.75f);
            else
                chargeBar.color = Color.white;
        }
        else if (isCharged && (releasedKey || (wasAttacking && !isAttacking)))
        {
            TempProjectile p;
            p = Instantiate(chargedProjectile, rb2D.position, Quaternion.identity);
            p.transform.Rotate(new Vector3(0, 0, 1), (180 * Mathf.Atan2(lastAttackDir.y, lastAttackDir.x)) / Mathf.PI - 90);
            p.speed = projectileSpeed * 1.1f;
            p.damageAmount = damage;
            p.lifeTime = projectileRange;
            chargeBar.color = Color.white;
            isCharged = false;
            chargedAttackTimer = 0;
            chargeBar.fillAmount = chargedAttackTimer / chargeTime;
            charges--;
        }
        else if (chargedAttackTimer > 0 && !Input.GetKey(KeyCode.Space))
        {
            chargeBar.fillAmount = chargedAttackTimer / chargeTime;
            if (chargeBar.fillAmount == 1) { chargeBar.color = new Color(1, 0, 0.75f); }
            else { chargeBar.color = Color.white; }
            chargedAttackTimer -= Time.deltaTime * 4;
            isCharged = false;
            if (chargedAttackTimer < 0) chargedAttackTimer = 0;
        }
        wasAttacking = isAttacking;
        lastAttackDir = attackDir;
    }

    protected void InvincibilityChecks()
    {
        //for testing
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
        else if (takeDamage) TakeDamage(1);
        else if (die) Die();
    }

    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Possess();
        }
        else
        {
            UpdateMovement();
            UpdateAttacking();
        }
        InvincibilityChecks();
    }

    protected virtual void FixedUpdate()
    {
        //rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
        rb2D.velocity = velocity * Time.fixedDeltaTime * 100;
    }
}
