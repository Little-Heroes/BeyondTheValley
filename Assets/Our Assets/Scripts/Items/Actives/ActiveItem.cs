using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItem : Item {

    public bool permanentStatUps;

    bool hasDropped = false;

    public override void OnPickUp(ref Player player) {
        if(!hasDropped) base.OnPickUp(ref player);
        //add it to the player, 
    }

    public void OnDrop(ref Player player) {
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
