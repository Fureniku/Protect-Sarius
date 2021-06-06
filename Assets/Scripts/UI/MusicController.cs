using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    
    //Create an object for music and keep it throughout the game. 
    //Ensures that more aren't created when returning to main menu.
    
    private static MusicController instance;
    void Start() {
        DontDestroyOnLoad(this);

        if (instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }
}
