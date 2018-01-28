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
	
	public GameObject fireworks;
	
	public Vector4 boundingBox;
	Rigidbody rb;

	public float jumpHeight = 13;
	public NetworkManager manager;

	[SyncVar] private int lives;
	[SyncVar] public float health = 0;
	
	public GameObject pistol;
	public GameObject sniper;
	public GameObject spawnAnim;
	public GameObject deathAnim;
	
	
	public int clas = 0;
	private int jumps = 0;
	private NetworkStartPosition[] spawnPoints;

	private GameObject gun;
	private gunControls gunControls;
	
	void Start () {
		GameObject g = GameObject.FindGameObjectWithTag("Fire");
		if (g != null) {
			boundingBox = g.GetComponent<BoundingBox>().bounds;
		}

		rb = GetComponent<Rigidbody> ();
		lives = maxLives;
//		var spawn = Instantiate(spawnAnim);
//		spawn.transform.localScale *= 3;
//		Destroy(spawn, 1f);
		if (isLocalPlayer) {
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
		
		switch (clas) {
			case 0:
				pistol.SetActive(true);
				gun = pistol;
				break;
			case 1:
				sniper.SetActive(true);
				gun = sniper;
				break;
		}

		gunControls = gun.GetComponent<gunControls>();
		GetComponent<NetworkTransformChild>().target = gun.transform;
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer>().material.color = Color.black;
	}

	void FixedUpdate () {
		transform.localScale = new Vector3(1 + 0.1F * Mathf.Pow(2, health), 1 + 0.1F * Mathf.Pow(2, health), 1 + 0.1F * Mathf.Pow(2, health));
		
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
		if (lives <= 1) {
			StartCoroutine("endGameTimer");
		}
		if (!isServer) return;
		if (lives <= 1) {
			CmdDeath();
			CmdgameOver();
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
		
		// Set the spawn point to origin as a default value
		Vector3 spawnPoint = Vector3.zero;

		// If there is a spawn point array and the array is not empty, pick a spawn point at random
		if (spawnPoints != null && spawnPoints.Length > 0) {
			spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
		}

		// Set the player’s position to the chosen spawn point
		transform.position = spawnPoint;
		
		rb.velocity = Vector3.zero;
	}

	[ClientRpc]
	public void RpcDamage() {
		health += gunControls.bulletDamage;
	}
	
	private void OnCollisionEnter(Collision other) {
		jumps = 0;
	}
	
	[Command]
    public void CmdFire() {
        var go = Instantiate(gunControls.bullet);
        
        go.transform.position = gunControls.bulletSpawn.position;
        go.GetComponent<Rigidbody>().velocity = gunControls.bulletSpawn.transform.forward * 10;
		go.GetComponent<Rigidbody>().transform.LookAt(gunControls.bulletSpawn.transform.forward * gunControls.bulletSpeed);
		
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

	[Command]
	void CmdgameOver() {
		var go = Instantiate(fireworks);
		
		NetworkServer.Spawn(go);
	}
	
	IEnumerator endGameTimer() {
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(0);
	}
}
