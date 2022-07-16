using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Round
{
   public string RoundName;
   public WaveManager WaveManager;
}
public class RoundManager : MonoBehaviour
{
   public List<Round> Rounds = new List<Round>();
   public static int currentRoundNumber = 0;
   private void Update()
   {
      var currentRound = Rounds[currentRoundNumber];
      
      Debug.Log(currentRound.RoundName);
   }
}
