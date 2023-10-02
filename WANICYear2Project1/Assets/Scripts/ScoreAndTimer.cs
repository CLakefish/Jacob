using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAndTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text text2;
    [SerializeField] private TMP_Text HighScoreText;
    public static ScoreAndTimer Singleton;
    public float currentScore { get; private set; }


    public float HighScore;
    public float PossibleScore;
    public float Multiplier;
    public float MulitplierTimer;
    private float timer = 0;

    public TMP_Text ScoreTXT;

    EnemySpawner EnemySpawner;

    void Start()
    {
        Singleton = this;
        text2.gameObject.SetActive(false);
        EnemySpawner = GetComponent<EnemySpawner>();
    }
    internal void Die()
    {
        if(currentScore > HighScore)
        {
            HighScore = currentScore;
        }
        ScoreTXT.text = "Score: " + currentScore + " HighScore: " + HighScore;
    }
    internal void GainPoints(int points)
    {
        PossibleScore += points * EnemySpawner.DifficultyRate;
        timer = MulitplierTimer;
        EnemySpawner.EnemiesKilledPerRaise++;
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
        HighScoreText.text = "H: " + HighScore;
    }

   internal void multiplyChain()
   {
        Multiplier++;
        timer = MulitplierTimer;
        EnemySpawner.EnemiesKilledPerRaise++;
    }
}
