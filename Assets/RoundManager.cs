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
   public static int currentRoundNum = 0;
}
