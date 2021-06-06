using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedFlash : MonoBehaviour {
    
    //Handle the red flashing animation when the player enters the warning zone

    [SerializeField] private Image img;
    [SerializeField] private float minTrans = 0.0f;
    [SerializeField] private float maxTrans = 0.3f;
    
    private bool increasing = true;
    private float transparency = 0f;

    void Update() {
        if (increasing) {
            if (transparency < maxTrans) {
                transparency += 0.005f;
            }
            else {
                increasing = false;
            }
        }
        else {
            if (transparency > minTrans) {
                transparency -= 0.005f;
            }
            else {
                increasing = true;
            }
        }

        Color tempCol = img.color;
        tempCol.a = transparency;

        img.color = tempCol;
    }
}
