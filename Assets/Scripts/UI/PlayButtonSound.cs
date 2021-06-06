using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour {
    
    //Object that holds the button press sound instance, persists across scenes.
    //Designed this way so when you press a button that loads a new scene, the sound will play as the scene loads.
    //Avoids a weird cutting out issue!
    
    private AudioSource audio;
    private static PlayButtonSound instance;

    void Start() {
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);

        if (instance != null) {
            Destroy(gameObject);
            Debug.Log("Exists already, kill the imposter");
        }
        else {
            instance = this;
            Debug.Log("Doesn't exist yet, use it");
        }
    }

    public void Play() {
        audio.PlayOneShot(audio.clip);
    }
}
