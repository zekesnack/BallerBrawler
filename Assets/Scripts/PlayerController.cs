using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject projectile;
	public GameObject bulletSpawnPoint;
	public float bulletSpeed = 1;
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

		if (Input.GetMouseButtonDown(0)) {
			Transform bulletspawn = bulletSpawnPoint.GetComponent<Transform>();
			GameObject proj = Instantiate(projectile);
			proj.transform.position = bulletspawn.position;
			proj.GetComponent<Rigidbody>().velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * bulletSpeed;

		}
	}

	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
}
