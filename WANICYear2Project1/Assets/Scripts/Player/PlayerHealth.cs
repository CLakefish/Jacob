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
    private Rigidbody2D rb;
     [SerializeField] public Slider HealthBar;
    // When the player dies
    protected override void OnDeath()
    {
        //Destroy(gameObject);
        HealthBar.value = 0;
        HealthBar.fillRect.gameObject.SetActive(false);
    }

    // When the player is hit
    protected override void OnHit()
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * 12, ForceMode2D.Impulse);
        HealthBar.value = currentHealth;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HealthBar.maxValue = maxHealth;
        HealthBar.fillRect.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //Hit();
        }
    }
}


