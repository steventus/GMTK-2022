using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    enum EnemyTypes { MeleeAI, RangedAI };

    public enum UpgradeEnemy { Base, UpgradeOne, UpgradeTwo, UpgradeThree, UpgradeFour };

    enum GunTypes { Shotgun, AutoFire, BurstFire, Sniper };

    [SerializeField] private bool isMovingRight = true;

    [SerializeField] Transform playerTransform;

    [SerializeField] EnemyTypes currentEnemyType;

    [SerializeField] public UpgradeEnemy currentUpgradeEnemy;

    [SerializeField] bool canCharge;

    [SerializeField] bool canDash;

    [SerializeField] bool IsAvailable = true;


    [SerializeField] float CooldownDuration = 1.0f;

    public float speed;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
        float distance = Vector2.Distance(this.gameObject.transform.position, playerTransform.position);
        float speed = 3f * Time.deltaTime;
        Vector3 offset = transform.position - playerTransform.position;
        var upgradeTypes = currentUpgradeEnemy;

        if (distance < 6f)
        {
            switch (upgradeTypes)
            {
                case UpgradeEnemy.Base:
                    transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed);
                    break;
                case UpgradeEnemy.UpgradeOne:
                    transform.position = Vector2.Lerp(transform.position, playerTransform.position, speed);
                    break;
                case UpgradeEnemy.UpgradeTwo:

                    transform.position = Vector2.Lerp(transform.position, playerTransform.position, speed);
                    break;
                case UpgradeEnemy.UpgradeThree:

                    transform.position = Vector3.Lerp(transform.position + offset, playerTransform.position, speed).normalized;
                    break;
                case UpgradeEnemy.UpgradeFour:
                    UseAbility();
                    break;
                default:
                    Debug.Log("Upgrade Types not valid");
                    break;
            }
        }

        if (currentUpgradeEnemy == UpgradeEnemy.UpgradeTwo)
        {
            StartCoroutine(ChargeAttack());
            canCharge = true;
        }
        else
        {
            canCharge = false;
        }

   
        /*Debug.Log(upgradeTypes);*/
        /*Debug.Log(canCharge);*/
    }

    public void RunFromPlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        var upgradeTypes = currentUpgradeEnemy;
        float speed = 10f * Time.deltaTime;


        if (distance < 3f)
        {
            switch (upgradeTypes)
            {
                case UpgradeEnemy.Base:

                    var enemyPosition = transform.position;
                    var playerPosition = playerTransform.position;
                    var dist = (enemyPosition - playerPosition).magnitude;
                    var mapped = Mathf.InverseLerp(10, 5, dist);
                    transform.position = Vector2.MoveTowards(enemyPosition, playerPosition, -mapped * speed);

                    break;
                case UpgradeEnemy.UpgradeOne:

                    transform.position = Vector2.Lerp(transform.position, playerTransform.position, speed);
                    break;
                default:
                    Debug.Log("Upgrade Types not valid");
                    break;
            }
        }

        if (currentEnemyType == EnemyTypes.RangedAI && currentUpgradeEnemy == UpgradeEnemy.UpgradeOne)
        {
            UseAbility();
        }
    }

    IEnumerator ChargeAttack()
    {
        if (canCharge)
        {
            Debug.Log("Charging.... ");
            yield return new WaitForSeconds(1f);
            Debug.Log("Finish Charging");
        }
    }

    void UseAbility()
    {
        // if not available to use (still cooling down) just exit
        if (IsAvailable == false)
        {
            return;
        }

        // made it here then ability is available to use...
        // UseAbilityCode goes here
        Transform saveCurrentPlayerPosition;
        saveCurrentPlayerPosition = playerTransform;
        transform.position = Vector2.Lerp(-transform.position, saveCurrentPlayerPosition.position, 2* speed);

        // start the cooldown timer
        StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        IsAvailable = false;
        Debug.Log("Start Cooldown");
        yield return new WaitForSeconds(CooldownDuration);
        IsAvailable = true;
    }

   /* public IEnumerator DashMovement()
    {
        yield return new WaitForSeconds(1f);
        speed = 1;
        transform.position = Vector2.Lerp(transform.position, playerTransform.position, speed);
        StartCoroutine(StopDashMovement());
    }

    public IEnumerator StopDashMovement()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(DashMovement());
    }*/
}