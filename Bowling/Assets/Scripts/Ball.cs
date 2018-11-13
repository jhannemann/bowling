// Bowling - A simple Unity 3D demo bowling game
// Copyright (C) 2018 Dr.-Ing. Jens Hannemann - j.hannemann@ieee.org
//
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

	public float speed;
	private Rigidbody rigidBody;
	private bool thrown = false;
	public float horizontalSpeed;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!thrown) {
			float xAxis = Input.GetAxis ("Horizontal");
			Vector3 position = transform.position;
			position.x += xAxis*horizontalSpeed;
			transform.position = position;
		}
		if (!thrown && Input.GetKeyDown (KeyCode.Space)) {
			thrown = true;
			rigidBody.isKinematic = false;
			rigidBody.velocity = new Vector3 (0, 0, speed);
		}
	}

	void FixedUpdate() {
		if (thrown && rigidBody.IsSleeping()) {
			SceneManager.LoadScene("Scene 1");
		}
	}
}
