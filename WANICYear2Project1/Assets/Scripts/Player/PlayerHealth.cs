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

    public GameObject DeathPanel;
    private bool isDead = false;
    private Image DeathImage;
    // When the player dies
    protected override void OnDeath()
    {
        //Destroy(gameObject);
        HealthBar.value = 0;
        HealthBar.fillRect.gameObject.SetActive(false);
        ScoreAndTimer.Singleton.Die();
        DeathPanel.SetActive(true);
        isDead = true;
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
        DeathPanel.SetActive(false);
        DeathImage = DeathPanel.GetComponent<Image>();
        DeathImage.color = new Color(DeathImage.color.r, DeathImage.color.g, DeathImage.color.b, 0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //Hit();
        }
        if (isDead)
        {
            while(DeathImage.color.a <= 1f)
            {
                DeathImage.color = new Color(DeathImage.color.r, DeathImage.color.g, DeathImage.color.b, +0.1f);
            }
        }

    }
}


