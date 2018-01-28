using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics.Eventing.Reader;
//using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour {
	public int maxJumps = 2;
	public int maxLives = 4;
	
	public GameObject projectile;
	public GameObject bulletSpawnPoint;
	public GameObject fireworks;
	public float bulletSpeed = 10;
	public float bulletDamage = 1;
	public GameObject bullet;
	public Transform bulletSpawn;
	public Vector4 boundingBox;
	Rigidbody rb;

	public float jumpHeight = 13;
	public NetworkManager manager;

	[SyncVar] private int lives;
	[SyncVar] public float health = 0;
	
	public GameObject child;
	public GameObject spawnAnim;
	public GameObject deathAnim;

	private int jumps = 0;
	
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
		
		if (!isLocalPlayer) return;

		rb.velocity = new Vector3 (Input.GetAxis ("Horizontal") * 10, rb.velocity.y, rb.velocity.z);

		if (Input.GetKeyDown("space") && jumps < maxJumps) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpHeight, rb.velocity.z);
			jumps++;
		}
	}

	private void death() {
		if (!isServer) return;
		if (lives <= 1) {
			CmdDeath();
			gameOver();
		}
		else {
			print("else " + lives);
			RpcRespawn();
			CmdDeath();
		}
	}

	[ClientRpc]
	void RpcRespawn() {
		lives--;
		
		if (!isLocalPlayer) return;
		
		var spawn = Random.Range(0, manager.spawnPrefabs.Count - 1);
		transform.position = manager.spawnPrefabs[spawn].transform.position;
		rb.velocity = Vector3.zero;
	}

	[ClientRpc]
	public void RpcDamage() {
		health+=bulletDamage;
	}
	
	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
	
	[Command]
    public void CmdFire() {
        var go = Instantiate(bullet);
        
        go.transform.position = bulletSpawn.position;
        go.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 10;
		go.GetComponent<Rigidbody>().transform.LookAt(bulletSpawn.transform.forward * bulletSpeed);
		
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
		print(lives);
		endGameTimer();
		Destroy(gameObject);
		Instantiate(fireworks);
	}
	IEnumerable endGameTimer() {
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(0);
	}
}
