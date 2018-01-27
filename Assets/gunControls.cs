using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class gunControls : NetworkBehaviour {

	// Point you want to have sword rotate around
	public Transform Player;

	// how far you want the sword to be from point
	public float armLength = 1f;

	private bool isPlayer = false;

	void Start() {
		// if the sword is child object, this is the transform of the character (or shoulder)
		Player = transform.parent.transform;
		isPlayer = Player.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer;
	}

	void Update() {
		if (!isPlayer)
			return;
		
		// Get the direction between the shoulder and mouse (aka the target position)
		var shoulderToMouseDir =
			Input.mousePosition - Camera.main.WorldToScreenPoint(Player.position);
		
		transform.LookAt((transform.position - transform.parent.position).normalized * 500);
	}
}