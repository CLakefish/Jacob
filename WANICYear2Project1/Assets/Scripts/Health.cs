/*
 * zak baydass
 * 10/28/2022
 * ths script doesnt get used, health system was reworked.
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
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
                    deathEffects = GetComponent<DeathEffects>();
                    if (deathEffects != null)
                    {
                        deathEffects.deathEvent.Invoke();
                    }
                    Destroy(gameObject);
                }
            }
        }
}
