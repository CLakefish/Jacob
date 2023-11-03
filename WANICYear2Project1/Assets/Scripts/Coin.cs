using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int MaxValue;
    private int Value;

    public GameObject ParticleEffect;
    public GameObject TextFloat;

    public float TimeToDie;
    // Start is called before the first frame update
    void Start()
    {
        Value = Random.Range(1, MaxValue);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TimeToDie > 0)
        {
            TimeToDie -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && collision.gameObject.GetComponent<ScoreAndTimer>())
        {
            //collision.gameObject.GetComponent<ScoreAndTimer>().CoinValue += Value;
            //collision.gameObject.GetComponent<ScoreAndTimer>().CoinTXT.text = "C: " + collision.gameObject.GetComponent<ScoreAndTimer>().CoinValue;
            GameObject Part =  Instantiate(ParticleEffect, gameObject.transform);
            Part.transform.parent = null;
            //GameObject Texs = Instantiate(TextFloat, gameObject.transform);
           // Texs.transform.parent = null; Texs.GetComponentInChildren<TMP_Text>().text = "" + Value;
            Destroy(gameObject);
        }
    }
    
}
