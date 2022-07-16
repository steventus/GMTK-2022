using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueHandler : MonoBehaviour

{
    public int currentRoundNum;
    public TMP_Text round_Label;

    public WaveManager[] _waveManager;
    private void Awake()
    {
        _waveManager = FindObjectsOfType<WaveManager>();
    }

    private void Update()
    {
        if (currentRoundNum == _waveManager.Length + 1)
        {
            if (round_Label != null)
            {
                round_Label.text = "Finished";
                //Messenger.Broadcast(GameEvent.RoundsFinishedEvent);
            }
            
           
        }
        else
        {
            if (round_Label != null)
                round_Label.text = currentRoundNum.ToString();
        }
       
    }
}
