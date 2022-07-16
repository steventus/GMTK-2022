using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public float enemyHealth;

    void Start()
    {
        enemyHealth = 100f;
    }

  
    void Update()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullet")
        {
            enemyHealth -= 10f;
            Debug.Log(enemyHealth);
            if (enemyHealth <= 0)
            {

                gameObject.SetActive(false);
            }
        }
    }


}
