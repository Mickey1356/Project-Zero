using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

	[SerializeField] private EntityType type = EntityType.BIGCAT;

	public EntityType EntType {
		get {
			return type;
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Detecting");
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Player detected.");
			PlayerScript.playerScript.PlayerTouched(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
