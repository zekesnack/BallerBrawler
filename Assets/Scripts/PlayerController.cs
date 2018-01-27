using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody rb;

	private int jumps = 0;

	public int maxJumps = 2;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
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
