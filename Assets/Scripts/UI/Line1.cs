using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line1 : MonoBehaviour {
    
    //Handles the first of the two lines that make up the tutorial UI.
    //Starts after a delay.
    
    private int startDelay = 30;
    private Image thisLine;
    private int delay = 0;

    void Start() {
        thisLine = GetComponent<Image>();
    }

    void FixedUpdate() {
        if (delay < startDelay) {
            delay++;
        }
        else {
            if (thisLine.fillAmount < 1) {
                thisLine.fillAmount += 0.1f;
            }
        }
    }
}
