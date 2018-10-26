using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowlScript : MonoBehaviour {
	public const float tempGrowlTime = 1f;
	//to fix/consider: if a cat is already ongoing immobilization, growling it will reset the timer. it can short circuit the immobilisation.
	//to fix: if a cat is already undergoing immobilisation, and you release then you growl again, the cat can wakeup and miss the second growl.

	private List<GameObject> catsAffected;
	private List<GameObject> catsQueuing;
	private List<float> catTimings;
	private List<float> catTimings2;

	void Awake() {
		catsAffected = new List<GameObject>();
		catTimings = new List<float> ();
		catsQueuing = new List<GameObject> ();
		catTimings2 = new List<float> ();

	}

	private void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log ("detected" + other.gameObject);
		GameObject obj = other.gameObject;

		//Debug.Log (other.GetComponent<GameObject> ().tag);
		if (obj != null && obj.tag == "Cat") {
			if (!obj.GetComponent<CatController>().ongoing) {
				catsAffected.Add (obj);
				catTimings.Add (0f);
			} else {
				obj.GetComponent<CatController> ().queuing = true;
				obj.GetComponent<CatController> ().shouldwait = true;
				catsQueuing.Add (obj);
				catTimings2.Add (0f);
			}


			obj.GetComponent<CatController> ().Freeze ();//the cat is frozen immediately, unless it is still being launched, then it will land frozen.
			obj.GetComponent<CatController> ().activateTouching(false); //cat cannot touch you.
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
			for (int j = 0; j < catTimings2.Count; j++) {
				catTimings2 [j] += Time.deltaTime;
			}
		}

		if (Input.GetKeyUp (KeyCode.Space) && finished == false) { //if you release space, begin the takeoff.
			finished = true;
			PlayerScript.player.GetComponent<PlayerScript>().UnFreeze ();
			StartCoroutine(Finish());
		}
	}

	CatController catt;

	IEnumerator Finish() {
		PlayerScript.player.GetComponent<PlayerScript> ().UnFreeze ();
		//unfreeze every cat, start immobilization.
		int i = 0;
		foreach (GameObject cat in catsAffected) {
			catt = cat.GetComponent<CatController> ();
			catt.UnFreeze ();
			catt.Immobilize (algo (catTimings [i]));
			i++;
		}
		//add timing to queuing cats
		int j = 0;
		foreach (GameObject cat in catsQueuing) {
			cat.GetComponent<CatController> ().AddTiming (algo (catTimings2 [j]));
			cat.GetComponent<CatController> ().shouldwait = false;
			j++;
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
