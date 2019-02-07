using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempProjectile : MonoBehaviour {

	public float speed;
	public int damageAmount;

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
                collGO.GetComponent<OrcKing>().TakeDamage(damageAmount, this.gameObject);
                break;
            case "Player":
                collGO.GetComponent<TempPlayerController>().TakeDamage(damageAmount);
                break;
        }
    }
}
