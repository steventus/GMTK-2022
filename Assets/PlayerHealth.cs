using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float curHealth;

    public UnityEvent onTakeDamage, onDeath;

    void Start()
    {
        curHealth = maxHealth;   
    }
    public void TakeDamage(float _delta)
    {
        curHealth -= _delta;

<<<<<<< Updated upstream
        if (curHealth <= 0) Death();
        else onTakeDamage.Invoke();
=======
        Debug.Log("Take Damage");
        if (curHealth <= 0)
        {
            //Messenger.Broadcast(GameEvent.PlayerDeathEvent);
            Death();
        }
        else
        {
            onTakeDamage.Invoke();
        }
        
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemyBullet")
        {
            collision.gameObject.SetActive(false);
            TakeDamage(10);

        }
>>>>>>> Stashed changes
    }

    private void Death()
    {
        onDeath.Invoke();
    }
}
