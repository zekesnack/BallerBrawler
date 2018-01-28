using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other) {
		Destroy(gameObject);
		other.transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);
		throw new System.NotImplementedException();
	}
}
