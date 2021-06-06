using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    [SerializeField] private float bulletSpeed = 1;
    private Rigidbody rb;
    private int aliveTime = 0;
    [SerializeField] private int maxAliveTime = 180;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void AddBulletSpeed(float additional) {
        bulletSpeed += additional;
    }

    private void FixedUpdate() {
        if (aliveTime < maxAliveTime) {
            aliveTime++;
        }
        else {
            Destroy(gameObject);
        }
        Vector3 movement = rb.transform.rotation * Vector3.up;
        rb.velocity = movement * bulletSpeed;
    }

    
    private void OnCollisionEnter(Collision other) {
        //If it's an asteroid destroy the bullet.
        //Doesn't check if asteroid is destroyed - if two bullets hit the asteroid together, both should be removed.
        if (other.gameObject.layer == 11) {
            Destroy(gameObject);
        }

        if (other.gameObject.layer == 9) {
            //Friendly fire!!!
            ShipController ship = other.gameObject.GetComponent<ShipController>();
            ship.DamageShipWithoutImmunity(2.5f); //2.5 per bullet (so usually 5), and wont protect the ship from being hit by big scary asteroids either.
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 11) {
            Destroy(gameObject);
        }
    }
}
