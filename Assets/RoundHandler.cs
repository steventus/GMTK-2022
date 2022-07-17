using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundHandler : MonoBehaviour

{
    public int currentRoundNum;
    public TMP_Text round_Label;


    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.SpawnNewRound, OnNewWaveSapwn);
    }
    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.SpawnNewRound, OnNewWaveSapwn);
    }
    
    public int EnemySpawnMax = 2;
    public int EnemySpawnMin = 0;

    private void Update()
    {
        // if (currentRoundNum == _waveManager.Length + 1)
        // {
        //     if (round_Label != null)
        //     {
        //         round_Label.text = "Finished";
        //         Messenger.Broadcast(GameEvent.RoundsFinishedEvent);
        //     }
        //     
        //    
        // }
        
        

        
        if (round_Label != null)
            round_Label.text = (currentRoundNum - 1).ToString();
       
    }


    public void OnNewWaveSapwn()
    {
        
    }
}
