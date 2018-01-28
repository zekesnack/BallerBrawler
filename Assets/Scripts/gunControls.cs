using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class gunControls : MonoBehaviour {
    // Point you want to have sword rotate around
    public Transform Player;

    // how far you want the sword to be from point
    public float armLength = 1f;

    private bool isPlayer;

    public GameObject bullet;
    
    public Transform bulletSpawn;

    void Start() {
        // if the sword is child object, this is the transform of the character (or shoulder)
        Player = transform.parent.transform;
        isPlayer = Player.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer;
    }

    void Update() {
        if (!isPlayer) return;
        // Get the direction between the shoulder and mouse (aka the target position)
        var shoulderToMouseDir =
            Input.mousePosition - Camera.main.WorldToScreenPoint(Player.position);

        shoulderToMouseDir.z = 0; // zero z axis since we are using 2d
        // we normalize the new direction so you can make it the arm's length
        // then we add it to the shoulder's position
        transform.position = Player.position + (armLength * shoulderToMouseDir.normalized);

        transform.LookAt((transform.position - transform.parent.position).normalized * 500);
        
        if (Input.GetAxis("Fire1") > 0) {
            var go = Instantiate(bullet);
            go.transform.position = bulletSpawn.position;
            go.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 10;
        }
    }
}