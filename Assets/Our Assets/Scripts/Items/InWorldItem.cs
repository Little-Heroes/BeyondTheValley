using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InWorldItem : MonoBehaviour {

    public Item thisItem;

    private void OnTriggerEnter2D(Collider2D c) {
        Player player = c.GetComponent<Player>();
        if (player != null) { thisItem.OnPickUp(player); }
    }
}
