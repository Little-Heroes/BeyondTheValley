using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    //the stats a player has can be upgraded

    [Tooltip("How much this will change the max health of the player")]
    public int healthChange;

    [Tooltip("Will this heal the player by the health change")]
    public bool healPlayer;

    [Tooltip("will this heal the player to full health on pickup")]
    public bool healPlayerToFull;

    [Tooltip("how much faster/slower will this make the player")]
    public float speedChange;

    [Header("Projectile Changes")]
    [Tooltip("how many seconds between shots will this change the players fire rate by")]
    public float shotDelayChange;

    [Tooltip("the lowest amount of time between shots for the payer will be changed by this , in seconds")]
    public float maxShotDelayChange;

    [Tooltip("the move speed of the players projectiles +/- this")]
    public float projectileSpeedChange;

    [Tooltip("the change to the players projectiles range")]
    public float projectileRangeChange;

    [Tooltip("Adds this many uses of the charged shot to the player")]
    public int numChargeShotsChange;

    [Tooltip("increases the players capacity for charged shots")]
    public int maxChargeShotsChange;

    protected bool hasPickedUp = false;

    public virtual void OnPickUp(ref Player player) {
        hasPickedUp = true;
        player.MaxHealth += healthChange;
        if (healPlayerToFull) player.Health = player.MaxHealth;
        else if (healPlayer) player.Health += healthChange;
        player.MoveSpeed += speedChange;
        player.FastestSecondsPerShot += maxShotDelayChange;
        player.SecondsPerShot += shotDelayChange;
        player.ProjectileSpeed += projectileSpeedChange;
        player.ProjectileRange += projectileRangeChange;
        player.MaxCharges += maxChargeShotsChange;
        player.Charges += numChargeShotsChange;
    }
}
