using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {
    
    //Handles the initial game over state, before loading the game over scene.
    //Basically responsible for the fade-out

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverSGA;
    
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI gameOverSGAText;
    
    private Image panelBg;

    private bool isGameOver = false;
    private float transparency = 0f;
    private int finalTimer = 0;

    private void OnEnable() {
        PlayerController.OnGameOver += OnGameOver;
        ShipController.OnGameOver += OnGameOver;
    }

    private void OnDisable() {
        PlayerController.OnGameOver -= OnGameOver;
        ShipController.OnGameOver -= OnGameOver;
    }

    void Start() {
        panelBg = gameOverPanel.GetComponent<Image>();
        gameOverText = GetComponent<TextMeshProUGUI>();
        gameOverSGAText = gameOverSGA.GetComponent<TextMeshProUGUI>();
    }

    //Gradually fade in the Game Over panel to hide the game
    void Update() {
        if (isGameOver) {
            if (transparency < 1f) {
                transparency += 0.005f;
                Color tempCol = panelBg.color;
                tempCol.a = transparency;

                panelBg.color = tempCol;
                gameOverText.color = tempCol;
                gameOverSGAText.color = tempCol;
            }
            else {
                finalTimer++;
            }

            //After the animation, load the scene itself.
            if (finalTimer == 180) {
                SceneManager.LoadSceneAsync("GameOver");
            }
        }
    }

    //Start the game over process when the event is called
    public void OnGameOver() {
        Cursor.lockState = CursorLockMode.None;
        isGameOver = true;
    }
}
