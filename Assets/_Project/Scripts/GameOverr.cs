using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverr : MonoBehaviour
{
    

    public ValueHandler ValueHandler;
    public TMP_Text roundsSurvived;
    public TMP_Text enemiesKilled;

    public UnityEvent OnRoundFinished;
    public int killedEnemies = 0;
    public void OnEnable()
    {
        Messenger.AddListener(GameEvent.RoundsFinishedEvent, RoundFinished);
        Messenger.AddListener(GameEvent.EnemyDeathEvent, OnEnemyDeath);
        Messenger.AddListener(GameEvent.PlayerDeathEvent, RoundFinished);

    }

    public void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.RoundsFinishedEvent,  RoundFinished);
        Messenger.RemoveListener(GameEvent.EnemyDeathEvent, OnEnemyDeath);
        Messenger.RemoveListener(GameEvent.PlayerDeathEvent, RoundFinished);

    }


    private void Update()
    {
        roundsSurvived.text = (ValueHandler.currentRoundNum -1).ToString();
    }

    void OnEnemyDeath()
    {
        killedEnemies++;
        enemiesKilled.text = killedEnemies.ToString();
    }


    void RoundFinished()
    {
        Cursor.visible = true;
        
        OnRoundFinished?.Invoke();
        Time.timeScale = 0;
    }

    public void ReloadScene()
    {
        Cursor.visible = false;
        
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
