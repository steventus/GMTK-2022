using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int curHealth;

    public int enemiesKilled;

    public bool touchByEnemies;
    

    public UnityEvent onTakeDamage, onInvulEnd, onDeath, onRegainHealth;

    public bool ShouldUseSO;
    public RegainSystemSO regainSystemSo;

    public float invulDur;
    private float invulTimer;
    private bool ifInvul = false;

    private int requredKill = 2;
    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.PlayerReGainHealth, RegainHealth);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.PlayerReGainHealth,RegainHealth);

    }

    void Start()
    {
        curHealth = maxHealth;   
    }

    

    public void TakeDamage(float _delta)
    {
        if (Time.time < invulTimer)
            return;

        curHealth -= 1;
        Messenger<int>.Broadcast(UiEvent.player_takeDamage, curHealth);

        Debug.Log("Take Damage");
        if (curHealth <= 0)
        {
            Reference.cam.ShakeCamera(0.25f,0.25f);
            Messenger.Broadcast(GameEvent.PlayerDeathEvent);
            Death();
            
        }
        else
        {
            Reference.cam.ShakeCamera(0.15f,0.15f);

            onTakeDamage.Invoke();
        }

        invulTimer = Time.time + invulDur;
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemyBullet")
        {
            collision.gameObject.SetActive(false);
            var damage = collision.GetComponent<DamgerBullet>().BulletData.damage;
            TakeDamage(damage);

        }
    }
    private void Death()
    {
        onDeath.Invoke();
        gameObject.SetActive(false);

    }
    
    public void RegainHealth()
    {
        enemiesKilled++;

        if(ShouldUseSO)
        {
            foreach (var healthSystem in regainSystemSo.RegainHealthSystems.Where(healthSystem =>
                         healthSystem.requiredKill == enemiesKilled))
            {
                curHealth += healthSystem.regainHealthAmount;
                Debug.Log("Regained Health" + healthSystem.regainHealthAmount);
            }

            return;
        }

        if (requredKill == enemiesKilled)
        {
            curHealth += 1;
            Messenger<int>.Broadcast(UiEvent.player_takeDamage, curHealth);
        }

        requredKill = enemiesKilled * 2;
       
    }
    
}
