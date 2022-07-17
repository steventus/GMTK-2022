using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent 
{
   public static readonly string EnemyDeathEvent = "OnEnemyDeath";
   public static readonly string PlayerDeathEvent = "OnPlayerDeath";
   public static readonly string RoundsFinishedEvent = "OnRoundFinished";
   public static readonly string PlayerReGainHealth = "OnRegainHealth";
   public static readonly string PlayerTakeDamage = "OnPlayerTakeDamage";
   public static readonly string SpawnNewRound = "OnSpawnNewRound";


}
