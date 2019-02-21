using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempProjectile : MonoBehaviour {

    [HideInInspector]
    public Vector3 addVel;
	public float speed;
	public int damageAmount;
    public bool stun = false;

	public float lifeTime;
	private float lifeTimeCD; // lifeTime Count Down
    private Vector3 baseVel;

	void Start(){
		lifeTimeCD = lifeTime;
        baseVel = transform.up * speed;
        transform.up = (baseVel + addVel).normalized;
	}

	void Update(){
        transform.position += (baseVel + addVel) * Time.deltaTime;
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
        DamagableObjects DO = collGO.GetComponent<DamagableObjects>();
        if(DO != null)
        {
            DO.takeHit(this);
        }
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
            case "AI":
                collGO.GetComponent<AI>().TakeDamage(damageAmount, stun);
                break;
            case "Boss":
                collGO.GetComponent<Boss>().TakeDamage(damageAmount, stun);
                break;
        }
        AI enemy = collGO.GetComponent<AI>();
        if (enemy == null) { return; }
        enemy.TakeDamage(damageAmount, stun);
    }
}
