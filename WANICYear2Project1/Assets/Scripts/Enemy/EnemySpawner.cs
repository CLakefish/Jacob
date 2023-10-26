using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;

    public Vector2 LeftSpawnPoint;
    public Vector2 RightSpawnPoint;

    public int EnemiesPerSpawn;
    private bool EnemiesCanSpawn;

    private float SpawnRate;
    public float MaxSpawnRate;

    public int EnemiesKilledPerRaise;
    public float RaiseValue;

    public float WaveBufferTime;
    private float waveBuffer;
    public TMP_Text WaveTXT;

    public float DifficultyRate;

    public TMP_Text DifficultyText;
    public TMP_Text EnemyTXT;

    public GameObject[] PowerupPrefabs;
    public Transform PowerupTransform;
    // Start is called before the first frame update
    void Start()
    {
        RaiseValue = 10;
    }

    // Update is called once per frame
    void Update()
    {

        if(EnemiesCanSpawn)
        {
            EnemySpawners();
        }

        if(waveBuffer < 0)
        {
            EnemiesCanSpawn = true;
            WaveTXT.gameObject.SetActive(false);
        }
        else
        {
            waveBuffer -=1 * Time.deltaTime;
            EnemiesCanSpawn = false;
            WaveTXT.gameObject.SetActive(true);
            WaveTXT.text = "Wave starts in: " + System.Math.Round(waveBuffer);
        }

        if (EnemiesKilledPerRaise >= RaiseValue)
        {
            IncreaseDifficulty();
        }
        DifficultyText.text = "S: " + DifficultyRate + "x";
        EnemyTXT.text = "E: " + Mathf.Round(RaiseValue - EnemiesKilledPerRaise);

    }

    public void EnemySpawners()
    {
        //for every nth second, spawn an enemy at a random point inbetween min spawn distance and max spawn distance, reset spawn rate;
        if (SpawnRate < 0)
        {
            for (int i = 0; i < EnemiesPerSpawn; i++)
            {
                SpawnEnemy();
            }

            SpawnRate = MaxSpawnRate;
        }
        else
        {
            SpawnRate = SpawnRate - 1 * Time.deltaTime;
        }
    }

    private void IncreaseDifficulty()
    {
        waveBuffer = WaveBufferTime;
        if (DifficultyRate < 4) //makes certain that no matter what the gmae will only take away atleast 4 seconds from the spawn rate. set ACTUAL spawn rate to be greater than 8.
        {
            MaxSpawnRate -= 0.5f;
        }
        DifficultyRate = DifficultyRate += 0.5f;

        EnemiesPerSpawn++;
        RaiseValue = 10 * DifficultyRate *DifficultyRate;
        EnemiesKilledPerRaise = 0;
        Instantiate(PowerupPrefabs[Random.Range(0, PowerupPrefabs.Length)], PowerupTransform);
    }
    private void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, LeftSpawnPoint, Quaternion.identity);
        Instantiate(EnemyPrefab,RightSpawnPoint, Quaternion.identity);

        Debug.Log("enemySpawned!");
    }
}
