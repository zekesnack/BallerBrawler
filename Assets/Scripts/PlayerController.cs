using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject projectile;
	public GameObject bulletSpawnPoint;
	public float bulletSpeed = 1;

	public GameObject bullet;
	public Transform bulletSpawn;
	
	Rigidbody rb;

	private int jumps = 0;

	public int maxJumps = 2;

	public GameObject child;

	public float jumpHeight = 13;

	[SyncVar]
	public float health = 0;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.black;
		
	}

	void FixedUpdate () {
		transform.localScale = new Vector3(1 + 0.1F * health, 1 + 0.1F * health, 1 + 0.1F * health);
		
		if (!isLocalPlayer) {
			return;
		}

		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetKeyDown("space") && jumps < maxJumps) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpHeight, rb.velocity.z);
			jumps++;
		}
	}

	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}

	[ClientRpc]
	public void RpcDamage() {
		if (!isServer) {
			return;
		}

		health++;
	}
	
	[Command]
    public void CmdFire() {
        var go = Instantiate(bullet);
         
        go.transform.position = bulletSpawn.position;
        go.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 10;

        NetworkServer.Spawn(go);
    }
}
