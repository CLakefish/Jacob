/*
 * Hugo Devant
 * 10.3.2022
 * Add this to game objects you want things to happen to when they die or get destroyed
 * works with health and damage on collide, can be made to work with others 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathEffects : MonoBehaviour
{
    public UnityEvent deathEvent = new();
    public GameObject createOnDeath;

    // Start is called before the first frame update
    void Start()
    {
        deathEvent.AddListener(OnDeath);
    }

    void OnDeath()
    {
        if(createOnDeath != null)
        {
            Instantiate(createOnDeath, transform.position, transform.rotation);
        }
    }
}
