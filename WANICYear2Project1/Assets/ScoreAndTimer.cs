using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAndTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text text;
    public static ScoreAndTimer Singleton;
    public int currentScore { get; private set; }


    public int HighScore;
    public int PossibleScore;
    public float MulitplierTimer;
    private float timer;

    void Start()
    {
        Singleton = this;
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
            
        } 
        else if(PossibleScore > 0)
        {
            currentScore += PossibleScore;
            PossibleScore = 0;
        }

    }

   internal void multiplyChain()
   {
        PossibleScore = PossibleScore*2;
   }
}
