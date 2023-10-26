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
    [SerializeField] public TMP_Text CoinTXT;

    public static ScoreAndTimer Singleton;
    public float currentScore { get; set; }
    public int CoinValue;

    public float HighScore;
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
        ScoreTXT.text = "Score: " + currentScore + " HighScore: " + HighScore;
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
        text.text = "Score: " + currentScore;
        HighScoreText.text = "H: " + HighScore;
    }
}
