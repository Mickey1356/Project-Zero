using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAbilities : MonoBehaviour {

	public const float defGrowlRadius = 3f;
	private float currentGrowlRadius = defGrowlRadius;
	private CircleCollider2D coll;
	[SerializeField] private GameObject growler;


	//growl, area of effect


	void Growl() {
		//sets player immobile
		coll = growler.GetComponent<CircleCollider2D>();
		coll.radius = currentGrowlRadius;
		//spawns growling Trigger
	}

	void Awake () {
		//reset radius of the growl.

		if (growler == null) {
			Debug.Log ("null");
		}
	}

	void Start () {
		
	}

	int counter;

	// Update is called once per frame
	void Update () {
		if (growler == null) { Debug.Log (counter++); }
		if (Input.GetKeyDown (KeyCode.Space) && growler != null) {
			Debug.Log("growler is not null " + counter++);
			Instantiate (growler, transform.position, transform.rotation);
		}
	}
}
