﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour {
	[SerializeField] private bool isThisMusic = true;
	public AudioClip shootSound;
	public AudioClip bark1;
	public AudioClip bark2;
	public AudioClip bark3;
	public AudioClip growl;


	[SerializeField] private AudioSource source;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;
	// Use this for initialization

	void Awake () {
	}

	void Start () {
		if (isThisMusic == true) {
			source.PlayOneShot (source.clip, 0.65f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isThisMusic == false) {
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("pressed fire");
				source.PlayOneShot (growl, 1f);
			}
			if (Input.GetMouseButtonUp (0)) {
				source.Stop ();
			}
			if (Input.GetMouseButtonDown (1)) {
				int i = Random.Range (0, 3);
				switch (i) {
				case 0:
					source.PlayOneShot (bark1, 0.5f);  
					break;
				case 1:
					source.PlayOneShot (bark2, 0.5f);  
					break;
				case 2:
					source.PlayOneShot (bark3, 0.5f);  
					break;
				}
			} 
		}
	}
}