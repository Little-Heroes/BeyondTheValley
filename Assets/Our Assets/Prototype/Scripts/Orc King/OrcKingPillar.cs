using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcKingPillar : OrcKing {

    [Header("Pillar")]

    public Colours myColour;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerProjectile")
        {
            GameObject go = collision.gameObject;
            OrcKingProjectile proj = go.AddComponent<OrcKingProjectile>();
            proj.ChangeColour(myColour);
        }
    }
}
