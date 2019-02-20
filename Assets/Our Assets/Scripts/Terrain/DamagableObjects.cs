using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObjects : MonoBehaviour {
    
    private enum SpawnType
    {
        None = 0,
        All,
        RandomOne,
        RandomAll,
        RandomX
    }

    [Range(0, 100)]
    public int health = 1;

    public GameObject hitParticles;

    public GameObject destroyParticles;

    [Tooltip("The game object that will be in this ones spot when it is destroyed eg burnt out fire replacing a fire")]
    public GameObject replacement;

    public List<GameObject> spawnAbles;

    [Range(0,1)]
    public float spawnChance = 0.05f;

    [Tooltip("used for the spawn x type")]
    public int x;

    [SerializeField]
    private SpawnType spawnType;

    public void takeHit(TempProjectile p)
    {
        if (p != null)
        {
            health -= p.damageAmount;
            if(health <= 0)  DoDead();
            else if (hitParticles != null)
            {
                GameObject go = 
                    Instantiate(hitParticles, transform.position, Quaternion.identity);
                Destroy(go, 3);
            }
        }
    }

    private void DoDead()
    {
        if (destroyParticles != null)
        {
            GameObject go = 
                Instantiate(destroyParticles, transform.position, Quaternion.identity);
            Destroy(go, 3);
        }
        if (spawnType == SpawnType.None) { }
        else if (spawnType == SpawnType.All)
        {
            for (int i = 0; i < spawnAbles.Count; i++)
            {
                Instantiate(spawnAbles[i], transform.position, Quaternion.identity);
            }
        }
        else if(spawnType == SpawnType.RandomAll)
        {
            for (int i = 0; i < spawnAbles.Count; i++)
            {
                if(Random.Range(0.0f, 1.0f) <= 0.05f)
                    Instantiate(spawnAbles[i], transform.position, Quaternion.identity);
            }
        }
        else if(spawnType == SpawnType.RandomOne)
        {
            int rand = Random.Range(0, spawnAbles.Count);
            Instantiate(spawnAbles[rand], transform.position, Quaternion.identity);
        }
        else if (spawnType == SpawnType.RandomX)
        {
            for (int i = 0; i < x; i++)
            {
                int rand = Random.Range(0, spawnAbles.Count);
                Instantiate(spawnAbles[rand], transform.position, Quaternion.identity);
            }
        }
        if (replacement != null) Instantiate(replacement, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
