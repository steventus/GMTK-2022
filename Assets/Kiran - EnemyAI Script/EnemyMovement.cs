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
    }

   
    public void ChasePlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (distance < 6f)
        {
            float speed = 3f * Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, speed);
        }
    }

    public void RunFromPlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        
        if (distance < 3f)
        {
            float speed = 10f * Time.deltaTime;
            var myPos = transform.position;
            var otherPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            var dist = (myPos - otherPos).magnitude;
            var mapped = Mathf.InverseLerp(10, 5, dist);
            transform.position = Vector2.MoveTowards(myPos, otherPos, -mapped * speed);
        }
    }
}
