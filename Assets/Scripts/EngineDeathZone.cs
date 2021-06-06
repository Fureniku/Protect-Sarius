using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineDeathZone : MonoBehaviour {
    
    //Kill player when entering the motherships engine flare.
    //Slightly different to the general killzone because it's on entry, not exit.
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.KillPlayer();
        }
    }
}
