using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatHide : MonoBehaviour { 

	[SerializeField] private GameObject hidingCat;

	[SerializeField] private float detectionRange = 1.5f;
	[SerializeField] private float nearRange = 3f;

	private GameObject player;
	private Vector3 playerPos = Vector3.zero;
	private Vector2 radiusVec = Vector2.zero;
	private bool isPlayerNear;
	private bool triggered;

	private bool isHiding = true;
	public bool IsHiding {
		get {
			return isHiding;
		}
	}


	// Use this for initialization
	void Start () {
		player = PlayerScript.player;
	}

	//Detection of player by virtue of proximity.


	// Update is called once per frame
	void Update () {
		if (player != null) {
			playerPos = player.transform.position;
			radiusVec = playerPos - transform.position;
			if ((radiusVec).magnitude < nearRange) {  //this can be used for certain effects for when player is near, e.g. catsounds.
				isPlayerNear = true;
			} else {
				isPlayerNear = false;
			}
			if ((radiusVec).magnitude < detectionRange && triggered != true) {
				triggered = true;
				
			}
		}

		if (triggered) {
			if ((radiusVec.x - transform.position.x) < 0f) {
				Vector3 scale = transform.localScale;
				scale.x = -1;
				transform.localScale = scale;
			}
			Instantiate (hidingCat, transform.position, transform.rotation);
			Destroy (gameObject);
		}

	}

	/*IEnumerator DetectionWaiter() {

	}*/
}
