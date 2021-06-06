using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    
    //Reads the health and updates the players health bar
    
    [SerializeField] private PlayerController player;
    private Image bar;
    
    void Start() {
        bar = GetComponent<Image>();
    }

    void Update() {
        float health = player.GetHealth();
        float maxHealth = player.GetMaxHealth();
        
        bar.fillAmount = health / maxHealth;
    }
}
