using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyAIManager : MonoBehaviour
{
    public float enemyHealth;

    [Space]
    public UnityEvent onTakeDamage, onDeath;

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
        enemyHealth -= _delta;

        if (enemyHealth <= 0)
        {
            Messenger.Broadcast(GameEvent.EnemyDeathEvent);
            Messenger.Broadcast(GameEvent.PlayerReGainHealth);
            
            onDeath.Invoke();
            gameObject.SetActive(false);
        }

        else onTakeDamage.Invoke();
    }
    
}
