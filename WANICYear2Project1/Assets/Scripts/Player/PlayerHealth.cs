/*
 * zak baydass
 * 9/20/2023
 * Player Health
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{

    // When the player dies
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    // When the player is hit
    protected override void OnHit()
    {

    }


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}


