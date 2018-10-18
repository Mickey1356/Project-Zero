using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	GameObject player;
	[SerializeField] private bool canMove;
	[SerializeField] private float speed;

	private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		player = PlayerScript.player;
	}

	void FixedUpdate () {
		if (canMove) {
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
