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

        if (curHealth <= 0) Death();
        else onTakeDamage.Invoke();
    }

    private void Death()
    {
        onDeath.Invoke();
    }
}
