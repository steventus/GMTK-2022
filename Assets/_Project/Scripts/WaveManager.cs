using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    
    public static List<GameObject> enemies;
    int completedWaves = 0;
    private int wavesInGame = 1;

    private bool isDone = false;
    
    public bool CompletedAllWaves => completedWaves > 10;
    
    private void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Fake").ToList().ToList();
    }

    

    private void Update()
    {
        if(enemies.Count == 0 && !CompletedAllWaves)
        {
            Debug.Log("All enemies are dead");
            completedWaves++;
        }

        if (CompletedAllWaves && !isDone)
        {
            Debug.Log("All Waves is completed");
            isDone = true;
        }
    }
}