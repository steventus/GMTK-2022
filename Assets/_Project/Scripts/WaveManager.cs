using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int enemiesNum;
    public GameObject[] Enemytype;
    public float timeBetweenEnemeySpawn;
}

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves;
    public Transform[] spawnPoints;

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;

    private bool canSpawn = true;
    private bool TrySpawn = false;

    private bool ShouldStop;
    private int myRoundNumber;
    private void Update()
    {
        if (!ShouldStop)
        {
            ShouldStop = true;
            
            currentWave = waves[currentWaveNumber];
            if (canSpawn && nextSpawnTime < Time.time)
            {
                SpawnWave();
            }

            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Fake");

            if (totalEnemies.Length != 0) return;
            if (currentWaveNumber + 1 != waves.Count)
            {
                if (!TrySpawn) return;
                SpawnNextWave();
                return;
            }

            RoundManager.currentRoundNumber++;
            Debug.Log("Completed waves");
           
        }
    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }


    void SpawnWave()
    {
        SpawnEnemyAtRandomPos();
        GetNextWave();

        if (currentWave.enemiesNum != 0) return;
        canSpawn = false;
        TrySpawn = true;

    }

    private void GetNextWave()
    {
        currentWave.enemiesNum--;
        nextSpawnTime = Time.time + currentWave.timeBetweenEnemeySpawn;
    }

    private void SpawnEnemyAtRandomPos()
    {
        GameObject randomEnemy = currentWave.Enemytype[Random.Range(0, currentWave.Enemytype.Length)];
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
    }
}