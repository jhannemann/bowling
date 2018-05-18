using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	public float distance = 10;
	public float height = 2;
    public GameObject objectToFollow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (objectToFollow == null)
			return;

		Vector3 destination = objectToFollow.transform.position;
		destination.x = 0.0f;
		destination.y += height;
		destination.z += distance;
		transform.position = destination;
	}
}
