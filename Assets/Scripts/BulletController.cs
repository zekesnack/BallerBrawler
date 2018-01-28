using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

	void damage(GameObject other) {
		if (!isServer)
			return;

		other.GetComponent<PlayerController>().RpcDamage();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {

			damage(other.gameObject);
			other.gameObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * other.gameObject.GetComponent<PlayerController>().health * 10);
		}

		if (!other.gameObject.CompareTag("Bullet")) {
			Destroy(gameObject);
		}
	}
}
