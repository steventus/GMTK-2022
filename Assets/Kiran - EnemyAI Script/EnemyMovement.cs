using System;
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

    [SerializeField] Rigidbody2D playerRb;

    [SerializeField] EnemyTypes currentEnemyType;

    [SerializeField] public UpgradeEnemy currentUpgradeEnemy;

    [SerializeField] bool canCharge;

    [SerializeField] bool dashed;

    [SerializeField] bool IsAvailable = true;

    [SerializeField] bool isTouchingPlayer;

    [SerializeField] bool isFlanking = false;

    [SerializeField] bool hasTakenDamage = false;

    [SerializeField] bool canTeleport = true;

    public float TeleportTime = -5f;

    public float TimeElapsed = 0f;

    public bool isFlipped = false;

    [SerializeField] float flippedTime = 0f;  

    [SerializeField] public LayerMask WallLayer;

    public Rigidbody2D rb;

    [SerializeField] float CooldownDuration = 10f;

    public float speed;

    [SerializeField] private float RotateSpeed = 3f;
    [SerializeField] private float Radius = 2f;

    private Vector2 _centre;
    private float _angle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        var GO = GameObject.FindGameObjectWithTag("Player");
        playerRb = GO.GetComponent<Rigidbody2D>();
        isTouchingPlayer = false;
        canCharge = true;
        rb.freezeRotation = true;
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

        TimeElapsed += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        /*if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }*/
       
        if (currentEnemyType == EnemyTypes.RangedAI && collision.CompareTag("Wall"))
        {
           
            if (!isFlanking)
            {
                this.transform.position = Vector2.zero;
            }
        }

        if (currentEnemyType == EnemyTypes.MeleeAI && collision.CompareTag("Wall"))
        {
            speed = 0;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      /*  if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }*/
    }

  

    public bool DetectWall()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 10f;

        //Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, WallLayer);
        if (hit.collider != null)
        {
            return true;
        }


        return false;
    }

    public static float FindDegree(int x, int y)
    {
        float value = (float)((Mathf.Atan2(x, y) / Math.PI) * 180f);
        if (value < 0) value += 360f;

        return value;
    }

    public void ChasePlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, playerRb.position);

        var upgradeTypes = currentUpgradeEnemy;
        _centre = playerRb.position;

        if (distance < 6f)
        {
            switch (upgradeTypes)
            {
                case UpgradeEnemy.Base:
                    
                    
                    speed = 3f;
          
                    if (distance > 1f)
                    {
                        rb.position = Vector2.MoveTowards(rb.position, playerRb.position, speed * Time.deltaTime);
                    }

                    break;
                case UpgradeEnemy.UpgradeOne:

                    if (TimeElapsed >= flippedTime)
                    {
                        flippedTime = TimeElapsed + 4f;
                        isFlipped = !isFlipped;

                        if (isFlipped)
                        {
                            speed = 5f;
                            
                        }
                        else
                        {
                            speed = 8f;
                        }
                    }


                    if (distance > 1f)
                    {
                        rb.position = Vector2.LerpUnclamped(rb.position, playerRb.position, speed * Time.deltaTime);
                    }

                    break;
                case UpgradeEnemy.UpgradeTwo:

                    //Enemy is not moving
                    //Enemy Delays (Charge)

                    speed = 0;

                    if (canCharge)
                    {
                        StartCoroutine(ChargeAttack());
                    }


                    break;
                default:
                    Debug.Log("Upgrade Types not valid");
                    break;
            }
        }

    }

    public void RunFromPlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        var upgradeTypes = currentUpgradeEnemy;
        speed = 10f * Time.deltaTime;
        isFlanking = false;

        switch (upgradeTypes)
        {
            case UpgradeEnemy.Base:

                if(distance < 3f)
                {
                    var enemyPosition = rb.position;
                    var playerPosition = playerRb.position;
                    var dist = (enemyPosition - playerPosition).magnitude;
                    var mapped = Mathf.InverseLerp(10, 5, dist);
                    rb.position = Vector2.MoveTowards(enemyPosition, playerPosition, -mapped * speed);
                }

                break;

            case UpgradeEnemy.UpgradeThree:

                isFlanking = true;

                if(distance > 3f)
                {
                    float cursor = playerRb.GetComponent<AimMouse>().aimCursor.eulerAngles.z + 180;
                    if (cursor > 180)
                    {
                        cursor = cursor - 360;
                    }
                    //Debug.Log(cursor);
                    //Debug.Log(cursor);

                    Vector2 target = new Vector2(Mathf.Sin(cursor), Mathf.Cos(cursor)).normalized * 5f + playerRb.position;
                    rb.position = Vector2.MoveTowards(rb.position, target, 3f * Time.deltaTime);
                }
                else
                {
                    _centre = playerRb.position;

                    _angle += 2.4f * Time.deltaTime;
                    var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * 2f;
                    rb.position = _centre + offset;
                    isFlanking = false;

                    
                }



                break;

            case UpgradeEnemy.UpgradeFour:

                if(TimeElapsed >= TeleportTime + 5f)
                {
                    rb.position = GenerateTeleportCoordinate();
                    TeleportTime = TimeElapsed;
  
                }

                break;
            default:
                Debug.Log("Upgrade Types not valid");
                break;
        }

    }

    IEnumerator ChargeAttack()
    {
       
        canCharge = false;
        //Debug.Log("Charging.... ");
        yield return new WaitForSeconds(5f);
        //Enemy is keeps tracking player position
        //Enemy saves player position
        //Enemy dash to the saves player location
        Transform saveCurrentPlayerPosition;
        saveCurrentPlayerPosition = playerRb.transform;
/*        rb.freezeRotation = true;*/
        rb.AddForce(new Vector2(saveCurrentPlayerPosition.position.x - rb.position.x, saveCurrentPlayerPosition.position.y - rb.position.y).normalized * 15f, ForceMode2D.Impulse);
        speed = 0;
        /*//Debug.Log("Finish Charging");*/
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero;
 /*       rb.freezeRotation = false;*/
        canCharge = true;
        /*Debug.Log("Cooldown Complete");*/
    }

    void MeleeDashToPlayer()
    {
        // if not available to use (still cooling down) just exit
        if (IsAvailable == false)
        {
            return;
        }

        // made it here then ability is available to use...
        // UseAbilityCode goes here
        /* Rigidbody2D saveCurrentPlayerPosition;
         saveCurrentPlayerPosition = playerRb;*/
        /*transform.position = Vector2.MoveTowards(transform.position, playerRb.position, 2 * speed);*/
       
       
        // start the cooldown timer
/*        StartCoroutine(CastTeleport());*/
    }

    public IEnumerator CastTeleport()
    {
        IsAvailable = false;
        /*Debug.Log("Starting Cooldown.... ");*/
        yield return new WaitForSeconds(5f);
        /*Debug.Log("Teleporting.... ");*/
        rb.position = GenerateTeleportCoordinate();
        IsAvailable = true;
        canTeleport = true;
        /*Debug.Log("Cooldown Complete");*/
    }

    public Vector2 GenerateTeleportCoordinate()
    {
        return new Vector2(UnityEngine.Random.Range(-6.5f, 5.5f), UnityEngine.Random.Range(-3.5f,3.5f));
    }

    private void CancelTeleport()
    {
        TeleportTime = TimeElapsed;

        /*StopCoroutine(CastTeleport());
        Debug.Log("Cancelling Teleport");
        canTeleport = false;
        StartCoroutine(CastTeleport());*/
    }

    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.PlayerTakeDamage, CancelTeleport);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.PlayerTakeDamage, CancelTeleport);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PlayerTakeDamage, CancelTeleport);
    }

}