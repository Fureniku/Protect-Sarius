using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PersistentData : MonoBehaviour {
    
    //This class holds data to be sent to the game over scene after the game finishes
    
    private Stopwatch clock = new Stopwatch();
    private int score;
    
    void Start() {
        
        clock.Start();
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable() {
        RockController.OnRockDestroyed += OnRockDestroyed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        RockController.OnRockDestroyed -= OnRockDestroyed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnRockDestroyed(int s) {
        score += s;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (String.Equals(scene.name, "MainMenu", StringComparison.OrdinalIgnoreCase)) {
            Destroy(gameObject);
        }

        if (String.Equals(scene.name, "GameOver", StringComparison.OrdinalIgnoreCase)) {
            clock.Stop();
        }

        //Reset values for another game
        if (String.Equals(scene.name, "Gameplay", StringComparison.OrdinalIgnoreCase)) {
            clock.Reset();
            clock.Start();
            score = 0;
        }
    }

    public int GetScore() {
        return score;
    }

    public string GetTime() {
        TimeSpan t = clock.Elapsed;
        if (t.Hours > 0) { //The player has too much free time on their hands... literally.
            return $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds:00}";
        }
        return $"{t.Minutes:00}:{t.Seconds:00}.{t.Milliseconds:000}";
    }
}
