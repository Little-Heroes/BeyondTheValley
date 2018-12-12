// THIS IS JUST A PLACEHOLDER PLAYER CONTROLLER SCRIPT UNTIL THE ACTUAL ONE IS BEING WORKED ON

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TempPlayerController : MonoBehaviour {

	public float speed;

	private Animator anim;
	private Rigidbody2D rb2D;
	private Vector2 moveVelocity;

	void Start(){
		rb2D = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update(){
		// Get Player Input
		Vector2 moveInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		moveVelocity = moveInput.normalized * speed;

		if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {
			anim.SetBool ("isWalking", true);
		} else {
			anim.SetBool ("isWalking", false);
		}
	}

	void FixedUpdate(){
		// Move the Player
		rb2D.MovePosition(rb2D.position + moveVelocity * Time.fixedDeltaTime);
	}
}
