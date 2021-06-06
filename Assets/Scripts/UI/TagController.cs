using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagController : MonoBehaviour {
    
    //Controls the tag spawning for the tutorial UI
    //Increases a timer, and every delayTime enables a tag on-screen, until all are done.

    [SerializeField] private GameObject[] tags;
    [SerializeField] private int delayTime = 60;

    private int index = 0;
    private int timer = 1;

    void FixedUpdate() {
        timer++;

        if (timer % delayTime == 0) {
            if (index < tags.Length) {
                GameObject obj = tags[index];
                obj.SetActive(true);
                index++;
            }
            else {
                gameObject.SetActive(false);
            }
        }
    }
}
