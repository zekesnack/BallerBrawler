using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunControls : MonoBehaviour {

	// Point you want to have sword rotate around
	public Transform Player;

	// how far you want the sword to be from point
	public float armLength = 1f;

	void Start() {
		// if the sword is child object, this is the transform of the character (or shoulder)
		Player = transform.parent.transform;
	}

	void Update() {
		// Get the direction between the shoulder and mouse (aka the target position)
		Vector3 shoulderToMouseDir =
			Input.mousePosition - Camera.main.WorldToScreenPoint(Player.position);
		//print(Camera.main.WorldToScreenPoint(transform.position));
		shoulderToMouseDir.z = 0; // zero z axis since we are using 2d
		// we normalize the new direction so you can make it the arm's length
		// then we add it to the shoulder's position
		transform.position = Player.position + (armLength * shoulderToMouseDir.normalized);
		//print(shoulderToMouseDir.normalized);
		
		Vector3 val = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		val.z = 0;
		val.y = transform.rotation.y;
		transform.LookAt(val);
		transform.Rotate(new Vector3(120+260, 0, 0));
		

	}
}