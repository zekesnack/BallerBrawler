using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody rb;

	private bool grounded = true;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetAxis ("Jump") > 0 && grounded) {
			rb.AddForce (0, 500, 0);
			grounded = false;
		}
	}

	private void OnCollisionEnter(Collision other) {
		grounded = true;
	}
}
