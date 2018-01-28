using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	Rigidbody rb;

	private int jumps = 0;

	public int maxJumps = 2;

	public GameObject child;

	[SyncVar]
	public Transform childtran;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.black;
	}

	void FixedUpdate () {
		if (!isLocalPlayer) {
			child.transform.position = childtran.position;
			child.transform.rotation = childtran.rotation;
			child.transform.localScale = childtran.transform.localScale;
			return;
		}

		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetKeyDown("space") && jumps < maxJumps) {
			rb.AddForce (0, 500, 0);
			jumps++;
		}
		
		// Get the direction between the shoulder and mouse (aka the target position)
		var shoulderToMouseDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

		shoulderToMouseDir.z = 0; // zero z axis since we are using 2d
		// we normalize the new direction so you can make it the arm's length
		// then we add it to the shoulder's position
		child.transform.position = transform.position + (1 * shoulderToMouseDir.normalized);

		child.transform.LookAt((child.transform.position - transform.position).normalized * 500);
		childtran = child.transform;
	}

	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
}
