using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	private const float timeUntilReset = 3.5f;

	private EntityType type = EntityType.DOG;
	private string killer = "???";

	public bool canMove = true;
	public bool canDie = true;
	public EntityType Type {
		get {
			return type;
		}
	}

	private bool dying = false;

	public EntityState state;

	public static GameObject player;
	public static PlayerScript playerScript;
	[SerializeField] private float speed;             //Floating point variable to store the player's movement speed.

	private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

	// Use this for initialization
	void Awake()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
		player = gameObject;
		playerScript = this;
	}

	//for stuff like growling and barking
	void Freeze() {
		canMove = false;
	}

	void UnFreeze() {
		canMove = true;
	}

	void Start() {
		state = EntityState.ALIVE;
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		//Store the current horizontal input in the float moveHorizontal.
		float moveHorizontal = Input.GetAxis ("Horizontal");

		//Store the current vertical input in the float moveVertical.
		float moveVertical = Input.GetAxis ("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		float movex = 0;
		float movey = 0;



		//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
		//rb2d.AddForce (movement * speed);
		if (canMove) {
			rb2d.velocity = movement * speed;
		}
		/*if (moveHorizontal == 0f) {
			movex = 0f;
		}
		if (moveVertical == 0f) {
			movey = 0f;
		}*/
		//movement = new Vector2 (movex, movey);
	}

	void Update() {
		if (state == EntityState.DEAD && dying == false ) {
			dying = true;
			Die();
		}
	}

	void Die() {
		Debug.Log ("You were killed by a " + killer); //different messages?
		GameManager.gameManager.Increment("deaths");
		canMove = false;
		//death animation, statistics etc.
		//wait for some time
		Debug.Log("Restarting level...");
		//yield return new WaitForSeconds(timeUntilReset); //don't know how to get this to work.
		//reset game
		GameManager.gameManager.RestartLevel ();
	}
		
	public void PlayerTouched(GameObject obj = null) {
		Debug.Log ("received");
		if (obj != null && obj.tag == "Cat" && obj.GetComponent<CatController> () != null) {
			EntityType ent = obj.GetComponent<CatController> ().EntType;
			if ((ent == EntityType.BIGCAT || ent == EntityType.SMALLCAT) && canDie == true) {
				switch (ent) {
				case (EntityType.BIGCAT):
					killer = "Big Cat";
					break;
				case (EntityType.SMALLCAT):
					killer = "Small cat";
					break;
				}
				state = EntityState.DEAD;

			}	
		}
	}
}
