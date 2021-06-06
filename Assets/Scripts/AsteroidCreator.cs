using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class AsteroidCreator : MonoBehaviour {

    [SerializeField] private int initialCreationSpeed = 1000;
    [SerializeField] private int creationSpeedIncrease = 1;
    [SerializeField] private int maxCreationSpeed = 100;
    [SerializeField] private GameObject objectToCreate;

    [SerializeField] private int widthRadius = 1000;
    [SerializeField] private int heightRadius = 1000;
    
    private int timeSinceLastCreation = 0;
    private int targetTime;
    
    void Start() {
        targetTime = initialCreationSpeed;
    }

    //Randomly, periodically create an asteroid and spread it across the X/Y axis
    void FixedUpdate() {
        if (timeSinceLastCreation < targetTime) {
            timeSinceLastCreation++;
        }
        else {
            Vector3 spawnPos = new Vector3(Random.Range(-widthRadius, widthRadius), Random.Range(-heightRadius, heightRadius),
                transform.position.z);
            Instantiate(objectToCreate, spawnPos, Quaternion.identity);
            if (targetTime > maxCreationSpeed) {
                targetTime -= creationSpeedIncrease;
            }

            timeSinceLastCreation = 0;
        }
    }
}
