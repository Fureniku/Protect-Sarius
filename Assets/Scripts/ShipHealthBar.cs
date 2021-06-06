using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHealthBar : MonoBehaviour
{
    //Get the ships health and display it as a UI element
    
    [SerializeField] private ShipController ship;
    private Image bar;
    
    void Start() {
        bar = GetComponent<Image>();
    }

    void Update() {
        float health = ship.GetHealth();
        float maxHealth = ship.GetMaxHealth();
        
        bar.fillAmount = health / maxHealth;
    }
}
