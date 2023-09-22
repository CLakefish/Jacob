/*
 * zak baydass
 * 9/20/2023
 * Player Health
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : Health
{
     [SerializeField] public Slider HealthBar;
    // When the player dies
    protected override void OnDeath()
    {
        //Destroy(gameObject);
        //HealthBar.value = 0;
        //HealthBar.fillRect.gameObject.SetActive(false);
    }

    // When the player is hit
    protected override void OnHit()
    {
        //HealthBar.value = currentHealth;
    }


    private void Start()
    {
        //HealthBar.maxValue = maxHealth;
        //HealthBar.fillRect.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Hit(1);
        }
    }
}


