using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject Player;
    public GameObject EnemyPrefab;

    public int SpawnDistance;
    public int MaxSpawnDistance;

    public int EnemiesPerSpawn;

    public float SpawnRate;
    public float MaxSpawnRate;

    public float DifficultyRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for every nth second, spawn an enemy at a random point inbetween min spawn distance and max spawn distance, reset spawn rate;
        if(SpawnRate < 0)
        {
            for(int i = 0; i < EnemiesPerSpawn; i++)
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
        if (DifficultyRate < 4) //makes certain that no matter what the gmae will only take away atleast 4 seconds from the spawn rate. set ACTUAL spawn rate to be greater than 8.
        {
            DifficultyRate = DifficultyRate += 0.5f;
            MaxSpawnRate -= 0.5f;
        }
        EnemiesPerSpawn++;
    }
    private void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, new Vector3(Player.transform.position.x - Random.Range(SpawnDistance, MaxSpawnDistance), 0, 0), Quaternion.identity);
        Instantiate(EnemyPrefab, new Vector3(Player.transform.position.x + Random.Range(SpawnDistance, MaxSpawnDistance), 0, 0), Quaternion.identity);

        Debug.Log("enemySpawned!");
    }
}
