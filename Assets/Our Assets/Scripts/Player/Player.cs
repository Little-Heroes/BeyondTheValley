using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
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

    public bool baseCanFly;

    [Header("Projectiles")]
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

    bool canFly;

    //regular shot
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

    TempProjectile chargedProjectile;
    #endregion the stored stats

    //-----------------------
    //access to the stats
    //-----------------------
    #region access to the stats
    public int MaxHealth { get; set; }

    public int Health { get; set; }

    public float MoveSpeed { get; set; }

    public bool CanFly { get; set; }

    public float FastestSecondsPerShot { get; set; }

    public float SecondsPerShot { get; set; }

    public float ProjectileSpeed { get; set; }

    public float ProjectileRange { get; set; }

    public TempProjectile Projectile { get; set; }

    public int MaxCharges { get; set; }

    public int Charges { get; set; }

    public float ChargeDamageMult { get; set; }

    public float ChargeSplashRadius { get; set; }

    public TempProjectile ChargedProjectile { get; set; }
    #endregion access to the stats
    #endregion stats

    public List<Item> startingItems;

    public ActiveItem heldItem;

    List<Item> items = new List<Item>();

    int numItems = 0;

    private void Awake() {
        #region applying stats 
        //health
        maxHealth = baseMaxHealth;
        health = baseHealth;

        if (health > maxHealth) health = maxHealth;

        //movement
        moveSpeed = baseMoveSpeed; 
        canFly = baseCanFly;

        //regular shot
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

        chargedProjectile = baseChargedProjectile;
        #endregion applying stats

        for (int num = 0; num < 10000; num++) { items.Add(new Item()); }

        int i = 0;
        foreach (Item item in startingItems) { items[i] = item; i++; }

    }


    private void OnTriggerEnter2D(Collider2D c) { if (c.GetComponent<Item>() != null) { items[numItems] = c.GetComponent<Item>(); numItems++; } }
}
