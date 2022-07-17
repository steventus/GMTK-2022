using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyAIManager : MonoBehaviour
{
    public float iniEnemyHealth;
    public float curEnemyHealth;

    [Space]
    public UnityEvent onTakeDamage, onDeath;
    private void Awake()
    {
        curEnemyHealth = iniEnemyHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullet")
        {
            collision.gameObject.SetActive(false);
            TakeDamage(10);
        }
    }

    private void TakeDamage(float _delta)
    {
        curEnemyHealth -= _delta;

        if (curEnemyHealth <= 0)
        {
            Reference.cam.ShakeCamera(0.25f,0.25f);

            
            Messenger.Broadcast(GameEvent.EnemyDeathEvent);
            Messenger.Broadcast(GameEvent.PlayerReGainHealth);

            onDeath.Invoke();
            gameObject.SetActive(false);
            
        }

        else
        {
            onTakeDamage.Invoke();
            Reference.cam.ShakeCamera(0.15f,0.15f);
        }
    }
    
}
