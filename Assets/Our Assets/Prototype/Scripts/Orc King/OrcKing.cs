using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcKing : MonoBehaviour {

    [SerializeField]
    private float health;

    [Header("Orc Spawning")]
    public GameObject orc;
    public Transform[] orcSpawns;
    [SerializeField]
    [Tooltip("Percent Chance")]
    private float chanceOfSpawningOrc;
    [SerializeField]
    private float orcSpawnCooldown;
    float orcSpawnTimer;

    public enum Colours
    {
        Blue, 
        Red,
        Yellow
    }
    [Header("Color/Shape Changing")]
    public Colours currentColours;
    public float changeColourCooldown;


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(orcSpawnTimer > 0)
        {
            orcSpawnTimer -= Time.deltaTime;
        }
        else
        {
            int rng = Random.Range(0, 101);
            if(rng < chanceOfSpawningOrc)
            {
                int randomSpawn = Random.Range(0, orcSpawns.Length);
                Instantiate(orc, orcSpawns[randomSpawn].transform.position, Quaternion.identity);
                orcSpawnTimer = orcSpawnCooldown;
            }
        }
	}

    public void TakeDamage(float amount, GameObject projectile)
    {
        health -= amount;
        if(health <= 0)
        {
            //winning;
        }
    }
}
