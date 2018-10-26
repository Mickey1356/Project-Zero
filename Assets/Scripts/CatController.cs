using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

	[SerializeField] private float bigCatTimeMultiplier = 0.5f;
	[SerializeField] private float bigCatTimeMax = 2f;
	[SerializeField] float dieLaunchSpeed = 6f;

	private Rigidbody2D rb2d;
	private MoveScript mvscript;
	[SerializeField] private float timeUntilTouching = 0.1f; //time taken after appearing until small cat can touch player. safety precautions
	[SerializeField] private EntityType type = EntityType.BIGCAT;
	public int catId;
	private static int currentCatId;

	private CatAI catai;
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
		catai = GetComponent<CatAI> ();
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

	public void AddTiming(float additional) {
		if (queue == null)
			queue = new Queue<float> ();
		queue.Enqueue (additional);
	}

	Queue<float> queue;
	public bool ongoing;
	public bool queuing;
	public bool timingavailable = false;
	public bool shouldwait;

	IEnumerator immobilize(float time) {
		ongoing = true;
		if (EntType == EntityType.SMALLCAT) {
			this.Freeze ();
			yield return new WaitForSeconds (time);
			while (queuing) {

				if (shouldwait) {
					yield return new WaitUntil (() => !shouldwait);
				}
				if (queue.Count == 0) {
					queuing = false;
					break;
				}
				yield return new WaitForSeconds (queue.Dequeue ());
			}
				activateTouching (true);
				this.UnFreeze ();
		}
		if (EntType == EntityType.BIGCAT) {
			this.Freeze ();
			float sum = 0f;
			yield return new WaitForSeconds (bigcatalgo(time));
			sum += bigcatalgo(time);
			while (queuing && sum < bigCatTimeMax) {
				if (shouldwait) {
					StartCoroutine(waitLunge2(1.8f));
					yield return new WaitUntil (() => !shouldwait); // or wait 1.8 seconds and he will break away, kill you.
				} 
				if (queue.Count == 0) {
					queuing = false;
					break;
				}
				float temp = sum;
				sum += bigcatalgo (queue.Dequeue ());
				if (sum > bigCatTimeMax) {
					temp = bigCatTimeMax - temp;
					yield return new WaitForSeconds (temp);
				} else {
					yield return new WaitForSeconds (queue.Dequeue ());
				}
			}
			queuing = false;
			activateTouching (true);
			this.UnFreeze ();
		}
		ongoing = false;
	}

	IEnumerator waitLunge2(float time) {
		yield return new WaitForSeconds (time);
		if (shouldwait) 
			shouldwait = false;
	}

	float bigcatalgo (float time) {
		float temp = time * bigCatTimeMultiplier;
		return (Mathf.Clamp (temp, 0f, bigCatTimeMax));
	}

	public void Freeze() {
		switch (EntType) {
		case EntityType.SMALLCAT:
			move.SetCanMove (false);
			break;
		case EntityType.BIGCAT:
			catai.SetCanMove(false);
			break;
		}
	}
		
	public void UnFreeze() {
		switch (EntType) {
		case EntityType.SMALLCAT:
			move.SetCanMove (true);
			break;
		case EntityType.BIGCAT:
			catai.SetCanMove(true);
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



	IEnumerator finish() {
		Destroy (GetComponent<MoveScript> ());
		Vector2 temp = transform.position - PlayerScript.player.transform.position;
		rb2d.bodyType = RigidbodyType2D.Kinematic;
		rb2d.velocity = dieLaunchSpeed * temp.normalized;
		yield return new WaitForSeconds (3f);
		//or slowly fades out, using the spriterenderer.color -- alpha value.
		Destroy (gameObject);
	}

	public void OnBarked() {
		if (EntType == EntityType.SMALLCAT) {
			GetComponent<SpriteRenderer> ().color = Color.red;
			activateTouching(false);
			//GetComponent<Collider2D> ().isTrigger = true;
			Destroy(GetComponent<Collider2D>());
			StartCoroutine(finish ());
		} else if (EntType == EntityType.BIGCAT) {
			//some sort of ... stop the cat for 1 second.
		}
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
