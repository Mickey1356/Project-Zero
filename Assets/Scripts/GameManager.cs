using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public static GameManager gameManager;

	//STATISTICS

	private float wins;
	public float Wins {
		get {
			return wins;
		}
	}

	private float deaths;
	public float Deaths {
		get {
			return deaths;
		}
	}

	private float kills;
	public float Kills {
		get {
			return kills;
		}
	}

	private float bigCatKills;
	public float BigCatKills {
		get {
			return bigCatKills;
		}
	}

	private float smallCatKills;
	public float SmallCatKills {
		get {
			return smallCatKills;
		}
	}

	//barks this round
	private float barksLocal;
	public float BarksLocal {
		get {
			return barksLocal;
		}
	}

	private float barksGlobal;
	public float BarksGlobal {
		get {
			return barksGlobal;
		}
	}


	private float growlsLocal;
	public float GrowlsLocal {
		get {
			return growlsLocal;
		}
	}

	private float growlsGlobal;
	public float GrowlsGlobal {
		get {
			return growlsGlobal;
		}
	}

	public void Increment(string type) {
		switch (type) {
		case ("deaths"):
			deaths++;
			break;
		case ("growlsLocal"):
			break;
		}
	}

	void Awake ()   
	{
		if (gameManager == null)
		{
			DontDestroyOnLoad(gameObject);
			gameManager = this;
		}
		else if (gameManager != this)
		{
			Destroy (gameObject);
		}
	}

	void DisplayAllStats() {

	}

	void QueryStats(string category) {

	}

	void Start() {
		player = PlayerScript.player;
	}

	void Update() {
		if (player == null) {
			player = PlayerScript.player;
		}
	}

	public void RestartLevel(string option = "default") {
		if (option == "default") {
			Debug.Log ("Restarting level...");
			StopAllCoroutines ();
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	// Use this for initialization

}
