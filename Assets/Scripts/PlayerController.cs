using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject projectile;
	public GameObject bulletSpawnPoint;
	public float bulletSpeed = 1;
	private int i = 0;
	Rigidbody rb;

	private int jumps = 0;

	public int maxJumps = 2;

	public GameObject child;

	public float jumpHeight = 13;

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
//			child.transform.position = childtran.position;
//			child.transform.rotation = childtran.rotation;
//			child.transform.localScale = childtran.transform.localScale;
			return;
		}

		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetKeyDown("space") && jumps < maxJumps) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpHeight, rb.velocity.z);
			jumps++;
		}


		if (Input.GetMouseButtonDown(0)) {
			++i;
			Debug.Log("mouse click" + i);
			GameObject proj = Instantiate(projectile);
			proj.GetComponent<Rigidbody>().velocity = new Vector3(2,0,0);
			

		}

	}

	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
}
