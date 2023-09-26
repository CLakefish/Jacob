using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAndTimer : MonoBehaviour
{
    public TMP_Text ScoreTXT;
    public int HighScore;
    public int score;
    private int PossibleScore;
    public float MulitplierTimer;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        //get past HighScore

        

    }

    // Update is called once per frame
    void Update()
    {
        //constanty update Timer and Score
        ScoreTXT.text = "Score: " + score;
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else if(PossibleScore > 0)
        {
            score = PossibleScore;
            PossibleScore = 0;
        }

    }

    void GainPoints(int score, int Chain)
    {
        PossibleScore += score * Chain;
        timer = MulitplierTimer;
    }
}
