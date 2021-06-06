using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    
    //Kills player when they leave the zone
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.KillPlayer();
        }
    }
}
