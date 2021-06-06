using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TagTextController : MonoBehaviour {
    
    //Animate the text object of tutorial UI tags
    
    [SerializeField] private string text;
    [SerializeField] private int writeDelay = 2;

    private int delayTime = 30;
    
    private int currentFrame;
    private string currentString = "";
    private int currentChar = 0;
    private int writeDelayTimer = 0;

    private bool idle = false;
    private TextMeshProUGUI textMesh;
    private AudioSource audio;

    void Start() {
        textMesh = GetComponent<TextMeshProUGUI>();
        audio = GetComponent<AudioSource>();
    }

    //Process text animation
    void Update() {
        if (!idle) {
            currentFrame++;
            if (currentFrame >= delayTime) {
                if (writeDelayTimer < writeDelay) {
                    writeDelayTimer++;
                }
                else {
                    //Each "tick" of the script, add a character and play the beepy sound
                    if (text.Length > currentString.Length) {
                        currentString += text[currentChar];
                        currentChar++;
                        writeDelayTimer = 0;
                        textMesh.text = currentString;
                        audio.PlayOneShot(audio.clip);
                    }
                    else {
                        //completed text animation
                        idle = true;
                    }
                }
            }
        }
    }
}
