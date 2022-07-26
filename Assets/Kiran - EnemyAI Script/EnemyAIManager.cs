using System.Collections;
using System.Collections.Generic;
using CameraShake;
using UnityEngine;
using UnityEngine.Events;
public class EnemyAIManager : MonoBehaviour
{
    public float iniEnemyHealth;
    public float curEnemyHealth;

    private Rigidbody2D rb;
    
    
    [Space]
    public UnityEvent onTakeDamage, onDeath, onSpawn;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            var bullet = collision.GetComponent<DamgerBullet>();
            
            
            TakeDamage(bullet.BulletData.damage);
            Knockback(bullet.directionForce, bullet.forceAmount);
        }
    }

    private void TakeDamage(float _delta)
    {
        curEnemyHealth -= _delta;
        CameraShaker.Presets.ShortShake2D();

        if (curEnemyHealth <= 0)
        {
            
            Messenger.Broadcast(GameEvent.EnemyDeathEvent);
            Messenger.Broadcast(GameEvent.PlayerReGainHealth);

            onDeath.Invoke();
            gameObject.SetActive(false);
            
        }

        else
        {
            Messenger.Broadcast(GameEvent.PlayerTakeDamage);
            onTakeDamage.Invoke();
        }
    }
    
    public void Knockback(Vector2 direction, float amount)
    {
        rb.AddForce(direction * amount, ForceMode2D.Force);
    }
}
