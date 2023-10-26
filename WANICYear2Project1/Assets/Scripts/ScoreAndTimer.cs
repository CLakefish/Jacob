using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAndTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text Scoretext;
    [SerializeField] private TMP_Text HighScoreText;
    [SerializeField] public TMP_Text CoinTXT;

    public static ScoreAndTimer Singleton;
    public float currentScore { get; set; }
    public int CoinValue;

    public float HighScore;
    public float Multiplier;
    public float MulitplierTimer;
    private float timer = 0;

    public TMP_Text DeathScoreTXT;
    //work

    EnemySpawner EnemySpawner;

    void Start()
    {
        Singleton = this;
        EnemySpawner = GetComponent<EnemySpawner>();
        if(GameObject.FindGameObjectWithTag("ScoreKeeper"))
        HighScore = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>().Highscore;
        CoinValue = 0;
    }
    internal void Die()
    {
        if(currentScore > HighScore)
        {
            HighScore = currentScore;
        }
        DeathScoreTXT.text = "Score: " + currentScore + " HighScore: " + HighScore;
        GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>().Highscore = HighScore;
        Time.timeScale = 0f;
    }
    internal void GainPoints(int points)
    {
        currentScore += points * EnemySpawner.DifficultyRate;
        EnemySpawner.EnemiesKilledPerRaise++;
    }

    // Update is called once per frame
    void Update()
    {
        //constanty update Timer and Score
        Scoretext.text = "Score: " + currentScore;
        HighScoreText.text = "H: " + HighScore;
    }
}
