using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line2 : MonoBehaviour {
    
    //Handles the second line from the tutorial UI.
    //Waits for the first to finish then immedietely starts
    //Gives the effect of a single line filling with an angle in the middle.
    
    [SerializeField] private Image prevLine;
    private Image thisLine;

    void Start() {
        thisLine = GetComponent<Image>();
    }

    void FixedUpdate() {
        if (prevLine.fillAmount >= 1) {
            if (thisLine.fillAmount < 1) {
                thisLine.fillAmount += 0.1f;
            }
        }
    }
}
