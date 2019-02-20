using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempProjectile : MonoBehaviour {

	public float speed;
	public int damageAmount;
    public bool stun = false;

	public float lifeTime;
	private float lifeTimeCD; // lifeTime Count Down

	void Start(){
		lifeTimeCD = lifeTime;
	}

	void Update(){
		transform.position += transform.up * speed * Time.deltaTime;
		lifeTimeCD -= Time.deltaTime;

		if (lifeTimeCD <= 0) {
			DestroyProjectile();
		}
	}

	void DestroyProjectile(){
		Destroy (gameObject);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        GameObject collGO = collision.gameObject;
        switch (collGO.tag)
        {
            case "Orc":
                collGO.GetComponent<OrcBoi>().TakeDamage(damageAmount);
                break;
            case "Orc King":
                collGO.GetComponent<OrcKing>().TakeDamage(damageAmount, gameObject);
                break;
            case "Player":
                collGO.GetComponent<Player>().TakeDamage(damageAmount);
                break;
            case "Ranged":
                collGO.GetComponent<AI>().TakeDamage(damageAmount, stun);
                break;
            case "Charge":
                collGO.GetComponent<Charge>().TakeDamage(damageAmount);
                break;
            case "FreeRoam":
                collGO.GetComponent<FreeRoam>().TakeDamage(damageAmount);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        Destroy(gameObject);
        AI enemy = c.GetComponent<AI>();
        if (enemy == null) { return; }
        enemy.TakeDamage(damageAmount, stun);
    }
}
