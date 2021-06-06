using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RockController : MonoBehaviour {
    
    //Handle the overall movement and existence of the asteroids in the game. Does not control spawning.

    public delegate void RockDestroyed(int i);
    public static event RockDestroyed OnRockDestroyed;
    
    [SerializeField] private GameObject fragment;
    [SerializeField] private int fragmentCount = 4;
    [SerializeField] private bool destroySelf = false;
    [SerializeField] private int points = 200;
    
    private Rigidbody rb;
    private float speed;
    
    //Newly created rocks are immune to damage for 1 second, to allow them to move away from the centre of the old rock on creation.
    private int immunity = 0;
    //Set to true when hit by a laser, to prevent it calling twice and double-spawning rocks.
    private bool breakRock = false;
    private bool markedForDeath = false;

    private MeshCollider collider;
    private Vector3 standardRotation;

    void Start() {
        rb = GetComponent<Rigidbody>();

        float minSpeed = 50f;
        float maxSpeed = 250f;
        
        //Adjust the max speed based on their mass; smaller asteroids should be slower, as they're harder to hit.
        if (rb.mass < 100) {
            maxSpeed = 200f;
        }

        if (rb.mass < 20) {
            maxSpeed = 100f;
        }
        
        //Set the rotation randomly
        rb.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        
        //Create random forces to move in a direction. Between min-max and either positive or negative, avoiding zero so all rocks have momentum.
        float randX = Random.Range(minSpeed, maxSpeed) * (Random.Range(0, 2) == 0 ? -1 : 1);
        float randY = Random.Range(minSpeed, maxSpeed) * (Random.Range(0, 2) == 0 ? -1 : 1);
        float randZ = Random.Range(minSpeed, maxSpeed) * (Random.Range(0, 2) == 0 ? -1 : 1);
        
        //Add the force and rotation, to give each rock a unique direction/spin as they move.
        rb.AddForce(new Vector3(randX, randY, randZ), ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)), ForceMode.Impulse);
        
        collider = GetComponent<MeshCollider>();
    }

    void FixedUpdate() {
        if (immunity < 60) {
            immunity++;
            if (immunity == 60) {
                if (collider != null) {
                    collider.isTrigger = false;
                }
            }
        }

        //For the tiny rocks that appear when you break the smallest gameplay rock
        //Also removes any bugged rocks (there shouldn't be any, but a safety net). Scales them down so they shrink away, then deletes them.
        if (markedForDeath || collider == null) {
            transform.localScale -= new Vector3(2f, 2f, 2f);

            if (transform.localScale.x <= 0 || transform.localScale.y <= 0 || transform.localScale.z <= 0) {
                Destroy(gameObject);
            }
        }

        Vector3 pos = gameObject.transform.position;
        
        //If it goes too far to the left/right of the mothership, destroy it for performance, the player cant reach it anyway.
        if (pos.x >= 2500 || pos.x <= -2500 || pos.y >= 2500 || pos.y <= -2500) {
            Destroy(gameObject);
        }

        //Break rock gets set to true when it should be broken.
        //Handled this way to avoid duplicating the smaller rocks spawned if it breaks twice in one frame.
        if (breakRock) {
            BreakRock(true);
        }
    }

    public void BreakRock(bool givePoints) {
        breakRock = false;
        BreakSound();
        gameObject.GetComponent<MeshCollider>().isTrigger = true;
        if (fragment != null) {
            for (int i = 0; i < fragmentCount; i++) {
                GameObject obj = Instantiate(fragment, transform.position, Quaternion.identity);
                if (!fragment.name.Contains("Tiny")) {
                    obj.GetComponent<MeshCollider>().isTrigger = true;
                }
            }
        }

        if (OnRockDestroyed != null && givePoints) {
            OnRockDestroyed(points);
        }
        Destroy(gameObject);
    }

    //Play the sound of the rock breaking.
    public void BreakSound() {
        GameObject source = transform.GetChild(0).gameObject;
        AudioSource sound = source.GetComponent<AudioSource>();
        sound.PlayOneShot(sound.clip);
        source.transform.parent = null;
        Destroy(source, 3);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 10) {
            breakRock = true;
        }

        //Handle physics damage to the player/mothership
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.DamagePlayer(other.relativeVelocity.magnitude * (rb.mass / 250));
        }

        if (other.gameObject.layer == 9) {
            ShipController ship = other.gameObject.GetComponent<ShipController>();
            ship.DamageShip(other.relativeVelocity.magnitude * (rb.mass / 250));
            BreakRock(false); //ship is big and strong and breaks any rock that hits it, even while ship is immune.
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10) {
            breakRock = true;
        }
    }

    //While the rock is still inside another rock (on fresh spawning), keep them immune
    private void OnCollisionStay(Collision other) {
        if (other.gameObject.layer == 11) {
            immunity = 0;
        }
    }

    //Set a rock to naturally despawn with animation
    public void MarkForDeath() {
        markedForDeath = true;
    }
}
