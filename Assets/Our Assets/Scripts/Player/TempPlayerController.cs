// THIS IS JUST A PLACEHOLDER PLAYER CONTROLLER SCRIPT UNTIL THE ACTUAL ONE IS BEING WORKED ON

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TempPlayerController : MonoBehaviour {

	[Header("Movement Variables")]
	public float speed;
	private Animator anim;
	private Rigidbody2D rb2D;
	private Vector2 moveVelocity;

	[Header("Shooting Variables")]
	public GameObject projectileObj;
	private float shootingTimer;
	public float timeBtwnShots;

	void Start(){
		rb2D = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update(){
		#region Getting Player Input
		// Get Player Input
		Vector2 moveInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		moveVelocity = moveInput.normalized * speed;
		#endregion

		#region Playing Animations when Moving
		if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {
			anim.SetBool ("isWalking", true);
		} else {
			anim.SetBool ("isWalking", false);
		}
		#endregion

		#region Shooting
		#region Diagonal Shooting
		// If the Up Arrow & the Right Arrow are being held down at the same time
		if (Input.GetKey(KeyCode.UpArrow) && (Input.GetKey(KeyCode.RightArrow))){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, -45.0f));
				shootingTimer = Time.time;
			}
		}
		// If the Up Arrow & the Left Arrow are being held down at the same time
		if (Input.GetKey(KeyCode.UpArrow) && (Input.GetKey(KeyCode.LeftArrow))){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, 45.0f));
				shootingTimer = Time.time;
			}
		}
		// If the Down Arrow & the Right Arrow are being held down at the same time
		if (Input.GetKey(KeyCode.DownArrow) && (Input.GetKey(KeyCode.RightArrow))){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, -135.0f));
				shootingTimer = Time.time;
			}
		}
		// If the Down Arrow & the Left Arrow are being held down at the same time
		if (Input.GetKey(KeyCode.DownArrow) && (Input.GetKey(KeyCode.LeftArrow))){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, 135.0f));
				shootingTimer = Time.time;
			}
		}
		#endregion

		#region Staright Shooting
		// If the Up Arrow is being held down
		if (Input.GetKey(KeyCode.UpArrow)){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
				shootingTimer = Time.time;
			}
		// Else if the Right Arrow is being held down
		} else if (Input.GetKey(KeyCode.RightArrow)){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, 270.0f));
				shootingTimer = Time.time;
			}
		// Else if the Down Arrow is being held down
		} else if (Input.GetKey(KeyCode.DownArrow)){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, 180.0f));
				shootingTimer = Time.time;
			}
		} else if (Input.GetKey(KeyCode.LeftArrow)){
			if (Time.time - shootingTimer > timeBtwnShots){
				GameObject projectile = Instantiate(projectileObj, transform.position, Quaternion.Euler(0.0f, 0.0f, 90.0f));
				shootingTimer = Time.time;
			}
		}
		#endregion

		#endregion
	}

	void FixedUpdate(){
		// Move the Player
		rb2D.MovePosition(rb2D.position + moveVelocity * Time.fixedDeltaTime);
	}
}
