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
}
