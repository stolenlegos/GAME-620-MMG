using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScripts : MonoBehaviour
{
    public Slider slider;
    //public PlayerEffects player;
    private int _maxHealth;
    private int _currentHealth;
    public GameObject player; 

    private void Start() {
        player = GameObject.FindWithTag("Player"); 
        _maxHealth = player.GetComponent<PlayerCombat>().health;
        slider.maxValue = _maxHealth;  
    }
    private void Update() {
        _currentHealth = player.GetComponent<PlayerCombat>().health; 
        slider.value = _currentHealth; 
    }
}
