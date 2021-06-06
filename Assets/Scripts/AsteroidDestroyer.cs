using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDestroyer : MonoBehaviour {
    
    //For the square behind the ship- destroy asteroids that hit it.
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 11) {
            Destroy(other.gameObject);
        }
    }
}
