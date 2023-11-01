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
    public AudioSource myAud;
    public AudioClip myHurtClip;
    [SerializeField] public Slider HealthBar;
    [SerializeField] private Image HitEffect;

    public GameObject DeathPanel;
    // When the player dies
    protected override void OnDeath()
    {
        if (DeathPanel.activeSelf) return;

        //Destroy(gameObject);
        HealthBar.value = 0;
        HealthBar.fillRect.gameObject.SetActive(false);
        DeathPanel.SetActive(true);
        ScoreAndTimer.Singleton.Die();
    }

    // When the player is hit
    protected override void OnHit()
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * 12, ForceMode2D.Impulse);
        HitEffect.color = new Color(1, 0, 0, 0.4f);
        HealthBar.value = currentHealth;
        myAud.PlayOneShot(myHurtClip);
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HealthBar.maxValue = maxHealth;
        HealthBar.fillRect.gameObject.SetActive(true);
        DeathPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            //Hit();
        }

        if (HitEffect != null) HitEffect.color = new Color(1, 0, 0, HitEffect.color.a - (2 * Time.deltaTime));
    }
    public void GainHealth()
    {
        maxHealth++;
        currentHealth = maxHealth;
        HealthBar.maxValue++;
        HealthBar.value = maxHealth;
    }
}


