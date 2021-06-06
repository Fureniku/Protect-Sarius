using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    
    //Quick/easy class to load a scene by its name
    public void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
