using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyAIManager : MonoBehaviour
{
    public float iniEnemyHealth;
    public float curEnemyHealth;

    [Space]
    public UnityEvent onTakeDamage, onDeath, onSpawn;
    private void Awake()
    {
        curEnemyHealth = iniEnemyHealth;
    }
    private void OnEnable()
    {
        onSpawn.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullet")
        {
            collision.gameObject.SetActive(false);
            var damage = collision.GetComponent<DamgerBullet>().BulletData.damage;
            TakeDamage(damage);
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
            Messenger.Broadcast(GameEvent.PlayerTakeDamage);
            onTakeDamage.Invoke();
            Reference.cam.ShakeCamera(0.15f,0.15f);
        }
    }
    
}
