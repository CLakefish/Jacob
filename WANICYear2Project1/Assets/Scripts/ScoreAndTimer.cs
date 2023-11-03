using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAndTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text Scoretext;
    [SerializeField] private TMP_Text HighScoreText;

    public static ScoreAndTimer Singleton;
    ScoreKeeper scoreKeeper;
    public int currentScore { get; set; }

    public float Multiplier;
    public float MulitplierTimer;

    public TMP_Text DeathScoreTXT;

    EnemySpawner EnemySpawner;

    void Start()
    {
        Singleton = this;
        EnemySpawner = GetComponent<EnemySpawner>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }
    internal void Die()
    {
        if (currentScore > scoreKeeper.Highscore && scoreKeeper)
        {
            scoreKeeper.Highscore = currentScore;
        }
        DeathScoreTXT.text = "Score: " + currentScore + " HighScore: " + scoreKeeper.Highscore;
        Time.timeScale = 0f;
    }
    internal void GainPoints(int points)
    {
        currentScore += Mathf.FloorToInt(points * EnemySpawner.DifficultyRate);
        EnemySpawner.EnemiesKilledPerRaise++;
    }

    // Update is called once per frame
    void Update()
    {
        //constanty update Timer and Score
        Scoretext.text = "Score: " + currentScore;
        HighScoreText.text = "H: " + (scoreKeeper ? scoreKeeper.Highscore : "0");
    }
}
