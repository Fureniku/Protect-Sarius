using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFXFinder : MonoBehaviour {
    private PlayButtonSound button;
    
    //Tries to find the object that makes a button noise
    //Used for UI scripts across various scenes, as button noise generator is a persistent object.
    
    void Start() {
        GameObject buttonObj = GameObject.Find("ButtonSound");

        if (buttonObj != null) {
            button = buttonObj.GetComponent<PlayButtonSound>();
        }
        else {
            Debug.LogWarning("Unable to find the button sound object; no more button sounds for this game.");
        }
    }

    //Play the sound if it can.
    public void Play() {
        if (button != null) {
            button.Play();
        }
    }
}
