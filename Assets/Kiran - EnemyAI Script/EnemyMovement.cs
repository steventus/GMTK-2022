using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    enum EnemyTypes { MeleeAI, RangedAI };

    enum MovementTypes { ChasePlayer, RunFromPlayer };

    enum GunTypes { Shotgun, AutoFire, BurstFire, Sniper };

    [SerializeField] private bool isMovingRight = true;

    [SerializeField] EnemyTypes currentEnemyType;

    void Start()
    {
       
    }

    void FixedUpdate()
    {

        var enemyTypes = currentEnemyType;

        switch (enemyTypes)
        {
            case EnemyTypes.MeleeAI:
                ChasePlayerEnemyMovement();
                break;
            case EnemyTypes.RangedAI:
                RunFromPlayerEnemyMovement();
                break;
            default:
                Debug.Log("Error");
                break;
        }

     /*   Debug.Log("=============");
        Debug.Log("Enemy Types: " + enemyTypes);
        Debug.Log("=============");*/

    }

   
    public void ChasePlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (distance < 6f)
        {
            float speed = 10f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, speed);

        }
    }

    public void RunFromPlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (distance < 6f)
        {
            float speed = 10f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, -1 * speed);
        }
    }


}
