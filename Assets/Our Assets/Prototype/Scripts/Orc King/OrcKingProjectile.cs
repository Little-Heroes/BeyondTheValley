using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcKingProjectile : OrcKing {

    public Colours projectileColour;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ChangeColour(Colours colour)
    {
        switch (colour)
        {
            case Colours.Blue:
                projectileColour = Colours.Blue;
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
                break;
            case Colours.Red:
                projectileColour = Colours.Red;
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
                break;
            case Colours.Black:
                projectileColour = Colours.Black;
                gameObject.GetComponentInChildren<Renderer>().material.color = new Color(0, 0, 0);
                break;
        }
    }
}
