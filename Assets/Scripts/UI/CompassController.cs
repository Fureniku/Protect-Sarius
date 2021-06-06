using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassController : MonoBehaviour {

    [SerializeField] private GameObject player;

    //Keeps the on-screen compass pointing forwards
    void Update() {
        Quaternion rot = player.transform.rotation;
        transform.rotation = Quaternion.Euler(-rot.x+90, -rot.y, -rot.z);
    }
}
