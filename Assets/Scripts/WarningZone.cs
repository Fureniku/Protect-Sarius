using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningZone : MonoBehaviour {

    [SerializeField] private Image redFlash;

    //If player exits the warning zone, activate the UI object
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            redFlash.gameObject.SetActive(true);
        }
    }
    
    //Deactivate it on re-entry
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            redFlash.gameObject.SetActive(false);
        }
        
    }
}
