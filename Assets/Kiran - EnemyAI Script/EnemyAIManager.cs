using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public float enemyHealth;

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

            gameObject.SetActive(false);
        }
    }
}
