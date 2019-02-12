using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active Item", menuName = "Active Item")]
public class ActiveItem : Item {

    public bool permanentStatUps;

    bool hasDropped = false;

    public override void OnPickUp(Player player) {
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
        //player.SwapActive(this)
    }

    public void OnDrop(Player player) {
        if (permanentStatUps || hasDropped) { return; }
        player.MaxHealth -= healthChange;
        player.MoveSpeed -= speedChange;
        player.FastestSecondsPerShot -= maxShotDelayChange;
        player.SecondsPerShot -= shotDelayChange;
        player.ProjectileSpeed -= projectileSpeedChange;
        player.MaxCharges -= maxChargeShotsChange;
        player.Charges -= numChargeShotsChange;
        hasDropped = true;
    }
}
