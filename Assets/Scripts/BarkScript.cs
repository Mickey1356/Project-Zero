using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkScript : MonoBehaviour {

	DogAbilities dogscript;
	Vector2 O;
	Vector2 R;

	float barktime = 0.1f;

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Cat") {
			Process (other.gameObject);
		}
	}

	void Process(GameObject obj) {
		//primitive triangle method
		/*Vector2 pm;
		Vector2 pmm;
		Vector2 p1;
		Vector2 rmVec;
		float r1;
		float rm;
		float thetaHalf;
		float theta;

		p1 = obj.transform.position;
		pm = dogscript.mousePoint;
		Vector2 temp = p1 - O;
		r1 = temp.magnitude;
		rmVec = pm - O;
		rm = rmVec.magnitude;
		pmm = O + ((r1 / rm) * rmVec);
		temp = pmm - p1;
		thetaHalf = Mathf.Asin ((temp.magnitude) / (2f * r1));
		theta = 2f * thetaHalf;*/


		//use dot product
		Vector2 pm;
		Vector2 rm;
		Vector2 p1;
		Vector2 r1;
		float theta;
		p1 = obj.transform.position;
		pm = dogscript.mousePoint;
		r1 = p1 - O;
		rm = pm - O;
		float dotp = Vector2.Dot (r1, rm);
		theta = Mathf.Acos (dotp / (r1.magnitude * rm.magnitude));


		if (theta < ((dogscript.barkAngleDegree/360f)*Mathf.PI)) {
			obj.GetComponent<CatController> ().OnBarked ();
		}
	}

	void Start() {
		O = transform.position;
		dogscript = PlayerScript.player.GetComponent<DogAbilities> ();

		StartCoroutine (WaitDie ());

	}

	IEnumerator WaitDie() {
		yield return new WaitForSeconds (barktime);
		Destroy (GetComponent<Collider2D> ());
		Destroy (gameObject);
	}
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		
	}
}
