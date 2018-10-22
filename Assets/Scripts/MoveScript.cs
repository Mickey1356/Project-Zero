using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	GameObject player;
	[SerializeField] private bool canMove;
	[SerializeField] private float speed;
	[SerializeField] private float launchChaseMultiplier = 1.2f;
	[SerializeField] private float normalChaseXSpeed = 1f;
	[SerializeField] private float launchYSpeed = 1f;
	[SerializeField] private bool disabled;

	private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void Start() {
		player = PlayerScript.player;
	}

	public void launchCat() {
		StartCoroutine (Launch ());
	}

	private Vector2 setLaunchVelocity() { //based on the hiding object. e.g. how high velocity if a cardboard box. no v at all?
		if (player == null) {
			player = PlayerScript.player;
		}
		Vector2 radiusVec = player.transform.position - transform.position;
		Vector2 temp = Vector2.zero;
		float plescape = player.GetComponent<Rigidbody2D> ().velocity.x;
		if (radiusVec.x < 0f) {
			if (plescape < 0f) {
				temp.x = plescape / launchChaseMultiplier;
			} else
				temp.x = -normalChaseXSpeed;
		} else if (plescape > 0) {
			temp.x = plescape / launchChaseMultiplier;
		} else {
			temp.x = normalChaseXSpeed;
		}
		temp.y = launchYSpeed;
		return temp;
	}

	private bool waitLanding;
	private bool landed = true;
	private float initHeight;

	IEnumerator Launch() {
		canMove = false;
		Quaternion rot = transform.rotation;
		rb2d.bodyType = RigidbodyType2D.Dynamic;
		Vector2 temp = setLaunchVelocity ();
		this.rb2d.velocity = temp;
		initHeight = transform.position.y;
		landed = false;
		yield return new WaitUntil (() => landed);

		this.rb2d.bodyType = RigidbodyType2D.Kinematic;
		//if cat is rotated weirdly, reset
		////transform.rotation = rot;
		//or,
		if (transform.rotation != rot) {
			StartCoroutine (reRotate (rot));
		}
		this.rb2d.velocity = Vector2.zero;
		canMove = true;

	}

	//this part about spinning cats can be a good dynamic and funny addition if you have a Mario Star powerup thing.
	private Quaternion initRot;
	private bool rotating;
	IEnumerator reRotate(Quaternion initial) {
		initRot = initial;
		rotating = true;
		yield return new WaitUntil (() => transform.rotation == initRot );
		rotating = false;
	}
	/*IEnumerator WaitLanding() {
		yield return new WaitForSeconds (5f);
	}*/

	void FixedUpdate () {
		if (rotating) {
			Debug.Log ("rotating using slerp...");
			transform.rotation = Quaternion.Slerp (transform.rotation, initRot, 0.5f);
		}

		if (!landed && transform.position.y < initHeight) {
			landed = true;
		}
		if (canMove && !disabled) {
			Vector2 direction = player.transform.position - gameObject.transform.position;
			direction.Normalize ();
			Vector2 move = direction * speed;
			rb2d.velocity = move;

		}
	}

	// Update is called once per frame
	void Update () {
		if (canMove) {
			
		}
	}
}
