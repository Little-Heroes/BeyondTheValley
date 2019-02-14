using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleProjectile : MonoBehaviour {

    public float moveSpeed;
    public float frequency;
    public float magnitude;
    public Vector3 direction;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = transform.position + (transform.right * Mathf.Sin(Time.time * frequency) * magnitude) + transform.up * moveSpeed * Time.deltaTime;
	}

    public void DealDamage()
    {

    }
}
