using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject projectile;
	public GameObject bulletSpawnPoint;
	public GameObject fireworks;
	public float bulletSpeed = 1;

	public GameObject bullet;
	public Transform bulletSpawn;

	public Vector4 boundingBox;
	
	Rigidbody rb;

	private int jumps = 0;

	public int maxJumps = 2;

	public GameObject child;

	public float jumpHeight = 13;

	public NetworkManager manager;

	public int maxLives = 3;

	private int lives;

	[SyncVar]
	public float health = 0;

	public GameObject spawnAnim;

	public GameObject deathAnim;
	
	void Start () {
		rb = GetComponent<Rigidbody> ();
		lives = maxLives;
//		var spawn = Instantiate(spawnAnim);
//		spawn.transform.localScale *= 3;
//		Destroy(spawn, 1f);
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.black;
	}

	void FixedUpdate () {
		transform.localScale = new Vector3(1 + 0.1F * health, 1 + 0.1F * health, 1 + 0.1F * health);
		
		if (transform.position.x < boundingBox.x || transform.position.x > boundingBox.z ||
		    transform.position.y < boundingBox.y || transform.position.y > boundingBox.w) {
			death();
		}
		
		if (!isLocalPlayer) {
			return;
		}

		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetKeyDown("space") && jumps < maxJumps) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpHeight, rb.velocity.z);
			jumps++;
		}
	}

	private void death() {
		if (!isServer) return;
		if (lives == 1) {
			
		}
		lives--;
		gameOver();
		RpcRespawn();
		CmdDeath();
	}

	[ClientRpc]
	void RpcRespawn() {
		if (!isLocalPlayer) return;
		
		var spawn = Random.Range(0, manager.spawnPrefabs.Count - 1);
		transform.position = manager.spawnPrefabs[spawn].transform.position;
		rb.velocity = Vector3.zero;
	}

	[ClientRpc]
	public void RpcDamage() {
		health++;
	}
	
	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
	
	[Command]
    public void CmdFire() {
        var go = Instantiate(bullet);
         
        go.transform.position = bulletSpawn.position;
        go.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 10;

        NetworkServer.Spawn(go);
    }

	[Command]
	void CmdDeath() {
		health = 0;
		
		var go = Instantiate(deathAnim);
         
		go.transform.position = transform.position;
		go.transform.LookAt(new Vector3(0, 0, 0));

		NetworkServer.Spawn(go);
	}

	void gameOver() {
		Instantiate(fireworks);
	}
}
