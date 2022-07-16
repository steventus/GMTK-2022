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
            round_Label.text = "Finished";
            return;
        }
        else
        {
            round_Label.text = currentRoundNum.ToString();
        }
       
    }
}
