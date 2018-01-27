using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	Rigidbody rb;

	private int jumps = 0;

	public int maxJumps = 2;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.black;
	}

	void Update () {
		if (!isLocalPlayer)
			return;
		
		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetKeyDown("space") && jumps < maxJumps) {
			rb.AddForce (0, 500, 0);
			jumps++;
		}
	}

	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
}
