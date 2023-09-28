using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAndTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text text2;
    public static ScoreAndTimer Singleton;
    public int currentScore { get; private set; }


    public int HighScore;
    public int PossibleScore;
    public int Multiplier;
    public float MulitplierTimer;
    private float timer = 0;

    void Start()
    {
        Singleton = this;
        text2.gameObject.SetActive(false);
    }

    internal void GainPoints(int points)
    {
        PossibleScore += points;
        timer = MulitplierTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //constanty update Timer and Score
        text.text = "Score: " + currentScore;
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
            text2.gameObject.SetActive(true);
            text2.text = PossibleScore + "*" + Multiplier;
            
        } 
        else if(PossibleScore > 0)
        {
            currentScore += PossibleScore * Multiplier;
            PossibleScore = 0;
            Multiplier = 1;
            text2.gameObject.SetActive(false);
        }

    }

   internal void multiplyChain()
   {
        Multiplier++;
        timer = MulitplierTimer;
    }
}
