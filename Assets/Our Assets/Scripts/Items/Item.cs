using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject {

    //the stats a player has can be upgraded
    #region stat changes
    #region health
    //health
    [Header("Health")]
    [Tooltip("How much this will change the max health of the player")]
    public int healthChange;

    [Tooltip("Will this heal the player by the health change")]
    public bool healPlayer;

    [Tooltip("will this heal the player to full health on pickup")]
    public bool healPlayerToFull;
    #endregion health
    #region movement
    //movement
    [Header("Movement")]
    [Tooltip("how much faster/slower will this make the player")]
    public float speedChange;

    [Tooltip("does this item grant flight, one is yes 0 is no")]
    [Range(0,1)] public int grantsFlight;
    #endregion movement
    #region projectile
    //projectile
    [Header("Projectile Changes")]
    [Tooltip("How much does this change the players damage")]
    public int damageChange;

    [Tooltip("how many seconds between shots will this change the players fire rate by")]
    public float shotDelayChange;

    [Tooltip("the lowest amount of time between shots for the payer will be changed by this , in seconds")]
    public float maxShotDelayChange;

    [Tooltip("the move speed of the players projectiles +/- this")]
    public float projectileSpeedChange;

    [Tooltip("the change to the players projectiles range")]
    public float projectileRangeChange;
    #endregion projectile
    #region chargedShot
    //charged shots
    [Header("Charge shots change")]
    [Tooltip("Adds this many uses of the charged shot to the player")]
    public int numChargeShotsChange;

    [Tooltip("increases the players capacity for charged shots")]
    public int maxChargeShotsChange;

    [Tooltip("The damage mult of the charged shot from the players regular damage")]
    public float chargeDamageMultChange;

    [Tooltip("The size change of the splash radius of the charged projectile")]
    public float chargeSplashRadiusChange;

    [Tooltip("the charge rate cap")]
    public float fastestChargeTimeChange;

    [Tooltip("the charge rate change for charged shots")]
    public float chargeTimeChange;

    #endregion chargedShot
    #endregion stat changes
    protected bool hasPickedUp = false;

    public virtual void Activate(Player player) { }

    public virtual void OnPickUp(Player player) {
        hasPickedUp = true;
        //health changes
        player.MaxHealth += healthChange;
        if (healPlayerToFull) player.Health = player.MaxHealth;
        else if (healPlayer) player.Health += healthChange;
        //movement changes
        player.MoveSpeed += speedChange;
        player.CanFly += grantsFlight;
        //projectile changes
        player.Damage += damageChange;
        player.FastestSecondsPerShot += maxShotDelayChange;
        player.SecondsPerShot += shotDelayChange;
        player.ProjectileSpeed += projectileSpeedChange;
        player.ProjectileRange += projectileRangeChange;
        //charged shot changes
        player.MaxCharges += maxChargeShotsChange;
        player.Charges += numChargeShotsChange;
        player.ChargeDamageMult += chargeDamageMultChange;
        player.FastestChargeTime += fastestChargeTimeChange;
        player.ChargeTime += chargeTimeChange;
        player.items.Add(this);
    }
}
