using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAbilities : MonoBehaviour {

	//next up: Gauge & regen for Barking and Growling.  how does big cat react bark? 
	//next next up: cats with varying speeds/ change paces? does growling affect?
	//Lighting -- 2D lighting in unity makes things look alot nicer. (after sprites added)
	//cats with abilities. pickups, powerups.

	[SerializeField] public float barkAngleDegree = 115f;
	[SerializeField] private int startingBarks = 4;
	private int currentBarks;
	[SerializeField] private float barkFreezeTime = 0.35f;

	public const float defGrowlRadius = 3f;
	private float currentGrowlRadius = defGrowlRadius;
	private CircleCollider2D coll;
	[SerializeField] private GameObject growler;
	[SerializeField] private GameObject barker;




	//growl, area of effect


	void Growl() {
		//sets player immobile
		PlayerScript.player.GetComponent<PlayerScript>().Freeze();
		coll = growler.GetComponent<CircleCollider2D>();
		coll.radius = currentGrowlRadius;
		Instantiate (growler, transform.position, transform.rotation);

		//spawns growling Trigger

		GameManager.gameManager.Increment ("growlsGlobal");
	}

	public Vector2 mousePoint;

	void Bark() {
		if (currentBarks > 0) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
			mousePoint = ray.origin;
			//bark sound
			StartCoroutine(BarkFreeze());
			Instantiate (barker, transform.position, transform.rotation);
			Debug.Log ("Bark " + (currentBarks - 1) );
			currentBarks--;
			//doesnt matter where you are facing, just bark. and then handle the sprite facing left or right etc etc. later
		}
	}

	IEnumerator BarkFreeze() {
		PlayerScript pl = PlayerScript.player.GetComponent<PlayerScript> ();
		pl.Freeze ();
		yield return new WaitForSeconds (barkFreezeTime);
		pl.UnFreeze ();
	}

	void Awake () {
		currentBarks = startingBarks;

		//reset radius of the growl.

		/*if (growler == null) {
			Debug.Log ("null");
		}*/
	}


	void Start () {
		
	}

	//int counter;

	// Update is called once per frame
	void Update () {
		//if (growler == null) { Debug.Log (counter++); }
		if (Input.GetKeyDown (KeyCode.Space) && growler != null) {
			//Debug.Log("growler is not null " + counter++);
			Growl();
		}

		if (Input.GetMouseButtonDown(0)) {
			Bark ();
		}
	}
}
