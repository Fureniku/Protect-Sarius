using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    
    //Class to handle the ship movement and properties
    
    private Rigidbody rb;
    private float currentSpeed = 1f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private int repairSpeed = 180;
    
    private float health;
    private int immunity;

    private int repairCooldown = 900;
    private int repairTimer;
    
    public delegate void GameOver();
    public static event GameOver OnGameOver;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        currentSpeed = startSpeed;
    }
    
    //Ship gradually speeds up over time, to make the game more difficult
    private void FixedUpdate() {
        if (currentSpeed < maxSpeed) {
            currentSpeed += 0.01f;
        }
        
        Vector3 movement = rb.transform.rotation * Vector3.left; //I may have modelled it facing the wrong way... :)
        rb.velocity = movement * (currentSpeed);

        //Game over if it's damaged too much
        if (health <= 0) {
            if (OnGameOver != null) {
                OnGameOver.Invoke();
            }
        }
        
        //Cooldown for immunity
        if (immunity > 0) {
            immunity--;
        }

        //Mothership has self repair if damage isn't taken for ~15 seconds
        if (health < maxHealth) {
            if (repairCooldown < 900) {
                repairCooldown++;
            }
            else {
                if (repairTimer < repairSpeed) {
                    repairTimer++;
                }
                else {
                    repairTimer = 0;
                    health += 1f;
                    if (health > maxHealth) {
                        health = maxHealth;
                    }
                }
            }
        }
    }

    //Damages the ship and applies a second of immunity to further damage
    public void DamageShip(float amt) {
        if (immunity == 0) {
            DamageShipWithoutImmunity(amt);
            immunity = 60;
        }
    }

    //Applies damage without the immunity period (eg from laser shots)
    public void DamageShipWithoutImmunity(float amt) {
        health -= amt;
        
        //Ship pauses self-repair after damage
        repairCooldown = 0;
        repairTimer = 0;
    }
    
    //Damage both the player and ship if they crash into eachother.
    //Probably the most common way for people to lose the game on their first playthrough! 
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            Debug.Log("Two ships collided with magnitude " + other.relativeVelocity.magnitude + " and mass " + rb.mass);
            player.DamagePlayer(other.relativeVelocity.magnitude / 10);
            DamageShip(other.relativeVelocity.magnitude / 4);
        }
    }

    public float GetHealth() {
        return health;
    }

    public float GetMaxHealth() {
        return maxHealth;
    }
}
