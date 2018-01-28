using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other) {
		other.gameObject.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity);
	}
}
