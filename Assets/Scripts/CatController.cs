using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

	private Rigidbody2D rb2d;
	private MoveScript mvscript;
	[SerializeField] private float timeUntilTouching = 0.1f; //time taken after appearing until small cat can touch player. safety precautions
	[SerializeField] private EntityType type = EntityType.BIGCAT;
	public int catId;
	private static int currentCatId;

	//private CatAI catai;
	private MoveScript move;


	[SerializeField] private bool canTouch = true;
	public bool CanTouch {
		get {
			return canTouch;
		}
	}
	private bool isLaunching;

	public EntityType EntType {
		get {
			return type;
		}
	}

	void Awake() {
		catId = currentCatId;
		currentCatId++;
		move = GetComponent<MoveScript> ();
	}

	// Use this for initialization
	void Start () {
		if (EntType == EntityType.SMALLCAT) {
			rb2d = GetComponent<Rigidbody2D> ();
			mvscript = GetComponent<MoveScript> ();
			//canTouch = false;
			//StartCoroutine (TouchWait ());
			mvscript.launchCat();
			//implement launch velocity based on hiding place e.g. bin or sewer cap.
		}
	}
		
	public void Immobilize(float time) {
		StartCoroutine (immobilize (time));
	}

	IEnumerator immobilize(float time) {
		if (EntType == EntityType.SMALLCAT) {
			this.Freeze ();
			yield return new WaitForSeconds (time);
			this.UnFreeze ();
		}
		if (EntType == EntityType.BIGCAT) {
			this.Freeze ();
			//yield return new WaitForSeconds (time);
			this.UnFreeze();
		}
	}

	public void Freeze() {
		switch (EntType) {
		case EntityType.SMALLCAT:
			move.SetCanMove (false);
			break;
		case EntityType.BIGCAT:
			//catai.SetCanMove(false);
			break;
		}
	}

	public void UnFreeze() {
		switch (EntType) {
		case EntityType.SMALLCAT:
			move.SetCanMove (true);
			break;
		case EntityType.BIGCAT:
			//catai.SetCanMove(true);
			break;
		}
	}

	IEnumerator TouchWait() {
		yield return new WaitForSeconds (timeUntilTouching);
		activateTouching (true);
	}

	public void activateTouching (bool input) {
		canTouch = input;
	}

	private bool rigid;

	void OnCollisionEnter2D(Collision2D other) {
		if (canTouch) { // if it is a small cat, it cannot touch after some time it appears out of hiding. for safety precautions, or gives some time before cat can touch.
			//Debug.Log ("Detecting");
			if (other.gameObject.tag == "Player") {
				Debug.Log ("Player detected.");
				PlayerScript.playerScript.PlayerTouched (gameObject);
			}
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (canTouch) { // if it is a small cat, it cannot touch after some time it appears out of hiding. for safety precautions, or gives some time before cat can touch.
			//Debug.Log ("Detecting");
			if (other.gameObject.tag == "Player") {
				Debug.Log ("Player detected.");
				PlayerScript.playerScript.PlayerTouched (gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (canTouch) { // if it is a small cat, it cannot touch after some time it appears out of hiding. for safety precautions, or gives some time before cat can touch.
			Debug.Log ("Detecting");
			if (other.gameObject.tag == "Player") {
				Debug.Log ("Player detected.");
				PlayerScript.playerScript.PlayerTouched (gameObject);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (canTouch) { // if it is a small cat, it cannot touch after some time it appears out of hiding. for safety precautions, or gives some time before cat can touch.
			Debug.Log ("Detecting");
			if (other.gameObject.tag == "Player") {
				Debug.Log ("Player detected.");
				PlayerScript.playerScript.PlayerTouched (gameObject);
			}
		}
	}
		

	void FixedUpdate() {
		if (isLaunching) {

			rb2d.bodyType = RigidbodyType2D.Dynamic;

		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
