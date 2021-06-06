using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    //Why does this even need a script, silly unity...
    //Closes the game.
    public void QuitGame() {
        Application.Quit();
    }
}
