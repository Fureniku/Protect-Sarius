using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class SticksController : MonoBehaviour {

    [SerializeField] private GameObject[] toHide;

    // Show or hide control elements. Players can hide controls if they're using an external controller.
    public void UpdateControlStatus() {
        bool show = PlayerPrefs.GetInt("showControls", 1) != 0;

        for (int i = 0; i < toHide.Length; i++) {
            toHide[i].SetActive(show);
        }
    }
}
