/*
 * zak baydass and Carson Lakefish
 * 9/20/2023
 * Health Base
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [Header("Health Parameters")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;

    [Header("Invulnerability")]
    [SerializeField] private bool hasInvulnerability;
    [SerializeField] protected float invulnerabilityTime;
    private float previousHitTime;
    internal Vector3 hitPoint;

    public void Hit(int damage, Vector3 position)
    {
        if (hasInvulnerability && Time.time <= invulnerabilityTime + previousHitTime) return;

        hitPoint = position;
        previousHitTime = Time.time;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath();
            return;
        }

        OnHit();
    }

    protected abstract void OnDeath();
    protected abstract void OnHit();

    // Previous Code
    /*
    [SerializeField]
        [Min(0)]
    public float currentHealth = 3;
        [Min(1)]
    [SerializeField]
    public int maxHealth = 3;
    public bool destroyAtZero = true;
    public float DamageTimer;

    private void FixedUpdate()
    {
      // HealthBarSlider.value = currentHealth; //sets health bar slider to the current health always
       // HealthBarSlider.maxValue = maxHealth;
        if (DamageTimer > 0)
            DamageTimer -= Time.deltaTime;
    }
    public void takeDamage(float damage) //deals damage based on getting a collision from a bullet
        {
        if(DamageTimer <= 0)
            currentHealth -= damage; //takes damage from a value
        DamageTimer = 0.2f;
            if (currentHealth <= 0)
            {
                //run death things here
                DeathEffects deathEffects = GetComponent<DeathEffects>();
                if (deathEffects != null)
                {
                    deathEffects.deathEvent.Invoke();
                }
                if (destroyAtZero) 
                {
                    Destroy(gameObject);
                }
            }
        }*/
}
