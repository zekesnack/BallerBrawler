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

    void Start() {
        // if the sword is child object, this is the transform of the character (or shoulder)
        Player = transform.parent.transform;
        isPlayer = Player.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer;
    }

    void Update() {
    }
}