using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBoss : MonoBehaviour {

    [Header("Rain fire(apples) ability")]
    public GameObject appleGO;
    public int numberOfApples;
    public float rainApplesCooldown;
    float rainApplesTimer;
    public Transform[] appleSpawns;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(rainApplesTimer > 0)
        {
            rainApplesTimer -= Time.deltaTime;
        }
        else
        {
            for(int i = 0; i < numberOfApples; i++)
            {
                Instantiate(appleGO, appleSpawns[i].position, Quaternion.identity);
            }
            rainApplesTimer = rainApplesCooldown;
        }
	}


}
