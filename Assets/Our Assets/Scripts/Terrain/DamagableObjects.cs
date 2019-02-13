using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObjects : MonoBehaviour {
    
    private enum SpawnType
    {
        All = 0,
        RandomOne,
        RandomAll,
        RandomX
    }

    [Range(0, 100)]
    public int health = 1;

    public GameObject destroyParticles;

    public List<GameObject> spawnAbles;

    [Range(0,1)]
    public float spawnChance = 0.05f;

    [Tooltip("used for the spawn x type, DO NOT MAKE LARGER THAN LIST SIZE")]
    public int x;

    [SerializeField]
    private SpawnType spawnType;

    private void OnCollisionEnter(Collision c)
    {
        TempProjectile proj = c.gameObject.GetComponent<TempProjectile>();
        if (proj != null)
        {
            health -= proj.damageAmount;
            if(health <= 0) { DoDead(); }
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
        if (spawnType == SpawnType.All)
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
            if (x > spawnAbles.Count) x = spawnAbles.Count;
            for (int i = 0; i < x; i++)
            {
                int rand = Random.Range(0, spawnAbles.Count);
                Instantiate(spawnAbles[rand], transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }
}
