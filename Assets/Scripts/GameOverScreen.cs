using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour {
    
    //Handles text animations on the Game Over scene

    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject scoreNumber;
    [SerializeField] private GameObject timeText;
    [SerializeField] private GameObject timeNumber;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playAgain;

    private GameObject dataObject;
    private PersistentData persistentData;
    private int timer = 0;

    void Start() {
        dataObject = GameObject.Find("PersistantData");
        if (dataObject == null) {
            Debug.Log("Cant find data object :(");
        }
        else {
            persistentData = dataObject.GetComponent<PersistentData>();
        }
        
        scoreNumber.GetComponent<TextMeshProUGUI>().SetText(""+persistentData.GetScore());
        timeNumber.GetComponent<TextMeshProUGUI>().SetText(persistentData.GetTime());
    }
    
    //Gradually add the final scores
    void FixedUpdate() {
        if (timer == 0) {
            scoreText.SetActive(true);
            scoreNumber.SetActive(true);
        }

        if (timer == 30) {
            timeText.SetActive(true);
            timeNumber.SetActive(true);
        }

        if (timer == 60) {
            mainMenu.SetActive(true);
            playAgain.SetActive(true);
        }
        
        if (timer < 100) {
            timer++;
        }
    }
}
