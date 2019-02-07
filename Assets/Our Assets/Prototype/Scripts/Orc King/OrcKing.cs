using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcKing : MonoBehaviour
{

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
        Yellow,
        NumberOfColours
    }
    [Header("Color/Shape Changing")]
    public Colours currentColours;
    public float changeColourCooldown;
    float changeColourTimer;
    public GameObject PillarParent;
    public float rotationSpeed;

    [Header("Smashy boi")]
    public float smashCooldown;
    float smashTimer;
    public GameObject projectile;
    public Animation smashWarningTimer;
    public float circleRadius;
    public int numberOfProjectiles;
    public float smashy2TimeDelay;
    float smashy2Timer;
    bool canStartNewSmash = true;


    // Use this for initialization
    void Start()
    {
        ChangeColour();
        smashTimer = smashCooldown;
        smashy2Timer = smashy2TimeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        PillarParent.transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));

        if (orcSpawnTimer > 0)
        {
            orcSpawnTimer -= Time.deltaTime;
        }
        else
        {
            //pick a random number from 1 to 100
            int rng = Random.Range(0, 101);
            Debug.Log("RNG = " + rng + ", Chosen chance = " + chanceOfSpawningOrc);
            //if the random number is less than the chance percentage of spawning an orc
            if (rng < chanceOfSpawningOrc)
            {
                //spawn an orc in one of the orc spawn points
                int randomSpawn = Random.Range(0, orcSpawns.Length);
                Instantiate(orc, orcSpawns[randomSpawn].transform.position, Quaternion.identity);
            }
            //reset orc spawn timer
            orcSpawnTimer = orcSpawnCooldown;
        }
        if (changeColourTimer > 0)
        {
            changeColourTimer -= Time.deltaTime;
        }
        else
        {
            ChangeColour();
            changeColourTimer = changeColourCooldown;
        }
        if (smashTimer > 0)
        {
            smashTimer -= Time.deltaTime;
        }
        else
        {
            //do the thing
            //for (int i = 0; i < numberOfProjectiles; i++)
            //{
            //    float angle = i * Mathf.PI * 2 / numberOfProjectiles;
            //    Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * circleRadius;
            //    GameObject go = Instantiate(projectile, pos, Quaternion.identity);
            //    go.tag = "OrcKingProjectile";
            //}
            if (canStartNewSmash)
            {
                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    float j = ((float)(i * 1) / numberOfProjectiles);
                    float angle = j * (Mathf.PI) * 2.0f;
                    float x = Mathf.Sin(angle) * circleRadius;
                    float y = Mathf.Cos(angle) * circleRadius;
                    Vector3 pos = new Vector3(x, y, 0) + transform.position;

                    GameObject go = Instantiate(projectile, pos, Quaternion.identity);
                    go.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * -angle, Vector3.forward);
                    TempProjectile tp = go.GetComponent<TempProjectile>();
                    tp.damageAmount = 1;
                    tp.speed = 15;
                    canStartNewSmash = false;
                }

            }
            smashy2Timer -= Time.deltaTime;

            if (smashy2Timer <= 0)
            {
                for (float i = 0.5f; i < numberOfProjectiles; i++)
                {
                    float j = ((float)(i * 1) / numberOfProjectiles);
                    float angle = j * (Mathf.PI) * 2.0f;
                    float x = Mathf.Sin(angle) * circleRadius;
                    float y = Mathf.Cos(angle) * circleRadius;
                    Vector3 pos = new Vector3(x, y, 0) + transform.position;

                    GameObject go = Instantiate(projectile, pos, Quaternion.identity);
                    go.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * -angle, Vector3.forward);
                    TempProjectile tp = go.GetComponent<TempProjectile>();
                    tp.damageAmount = 1;
                    tp.speed = 15;
                    smashTimer = smashCooldown;
                    canStartNewSmash = true;
                    smashy2Timer = smashy2TimeDelay;
                }
            }
        }
    }

    public void TakeDamage(float amount, GameObject projectile)
    {
        if (projectile.GetComponent<OrcKingProjectile>())
        {
            OrcKingProjectile proj = projectile.GetComponent<OrcKingProjectile>();
            if (proj.projectileColour == currentColours)
            {
                health -= amount;
                if (health <= 0)
                {
                    //winning;
                }
            }
        }
    }

    public void ChangeColour()
    {
        int randomColour = Random.Range(0, (int)Colours.NumberOfColours);
        currentColours = (Colours)randomColour;

        switch (currentColours)
        {
            case Colours.Blue:
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case Colours.Red:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case Colours.Yellow:
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }
}
