using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowlScript : MonoBehaviour {
	public const float tempGrowlTime = 1f;


	private List<GameObject> catsAffected;
	private List<float> catTimings;

	void Awake() {
		catsAffected = new List<GameObject>();
		catTimings = new List<float> ();
	}

	private void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log ("detected" + other.gameObject);
		GameObject obj = other.gameObject;

		//Debug.Log (other.GetComponent<GameObject> ().tag);
		if (obj != null && obj.tag == "Cat") {
			catsAffected.Add (obj);
			catTimings.Add (0f);
			obj.GetComponent<CatController> ().Freeze ();//the cat is frozen immediately
			//CatController catt = obj.GetComponent<CatController> ();
		}
	}



	/*private void OnTriggerStay2D (Collider2D other) {
		Debug.Log ("detected" + other.gameObject);
		GameObject obj = other.gameObject;
		//Debug.Log (other.GetComponent<GameObject> ().tag);
		if (obj != null && obj.tag != "Player") {
			CatController catt = obj.GetComponent<CatController> ();

		}
	}*/

	float algo(float input) {
		if (input > 1f) {
			return input * input;
		} else if (input > 0f && input < 0.2f) {
			return 0.2f;
		} else {
			return input;
		}
	}

	void Start() {
	}

	bool finished;

	void Update() {
		if (finished == false) {
			for(int i = 0; i < catTimings.Count; i++) {
				catTimings [i] += Time.deltaTime;
			}
		}

		if (Input.GetKeyUp (KeyCode.Space) && finished == false) { //if you release space, begin the takeoff.
			finished = true;
			StartCoroutine(Finish());
		}
	}

	CatController catt;

	IEnumerator Finish() {
		//unfreeze every cat, start immobilization.
		int i = 0;
		foreach (GameObject cat in catsAffected) {
			catt = cat.GetComponent<CatController> ();
			catt.UnFreeze ();
			catt.Immobilize (algo (catTimings[i]));
			i++;
		
		
		}

		yield return new WaitForSeconds (0.001f);
		Destroy (gameObject);
		Destroy (this);

	}

	/*IEnumerator CountDown() {
		yield return new WaitForSeconds (tempGrowlTime);
		//Unfreeze every cat, start immobilization.
		Destroy (gameObject);
		Destroy (this);
	}*/
}
