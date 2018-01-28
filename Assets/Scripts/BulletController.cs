using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Player")) {
			other.transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);
			other.gameObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * other.transform.localScale.magnitude);
		}
		Destroy(gameObject);
	}
}
