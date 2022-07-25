using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    enum EnemyTypes { MeleeAI, RangedAI };

    public enum UpgradeEnemy { Base, UpgradeOne, UpgradeTwo, UpgradeThree, UpgradeFour };

    [SerializeField] Rigidbody2D playerRb;

    [SerializeField] EnemyTypes currentEnemyType;

    [SerializeField] public UpgradeEnemy currentUpgradeEnemy;

    [SerializeField] public List<UpgradeEnemy> correctUpgradesToShowCrown;
    
    [SerializeField] bool canCharge;
    
    [SerializeField] bool isFlanking = false;

    public float TeleportTime = -5f;

    public float TimeElapsed = 0f;

    public bool isFlipped = false;

    [SerializeField] float flippedTime = 0f;  

    [SerializeField] public LayerMask WallLayer;

    public Rigidbody2D rb;

    public float speed;

    [SerializeField] public Animator anim;

    private Vector2 _centre;
    private float _angle;
    public float transparency = 0f;
    public float targetTransparency = 0f;
    public Material mat;
    public UnityEvent onNormalSpawn, onUpgradeSpawn;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var GO = GameObject.FindGameObjectWithTag("Player");
        playerRb = GO.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
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

        if (collision.CompareTag("Player"))
        {
            float health = 10f;
            playerRb.GetComponent<PlayerHealth>().TakeDamage(health);
            Debug.Log(playerRb.GetComponent<PlayerHealth>().curHealth);
        }
       
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
    
    #region Melee AI Functions
    
    public void ChasePlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, playerRb.position);

        var upgradeTypes = currentUpgradeEnemy;
        _centre = playerRb.position;

        if (distance < 12f)
        {
            switch (upgradeTypes)
            {
                case UpgradeEnemy.Base:
                    
                    speed = 3f;
                    
                    if (distance > 1f)
                    {
                        rb.position = Vector2.MoveTowards(rb.position, playerRb.position, speed * Time.deltaTime); //Move Enemy to Player
                        //var dir = playerRb.transform.position - rb.transform.position; //Calculate Player and Enemy direction
                        //transform.up = Vector2.Lerp(-transform.forward, dir, Time.deltaTime * 3f); //Rotate towards player
                        
                        // var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }
                    
                    break;
                case UpgradeEnemy.UpgradeOne:
                    
                    if (TimeElapsed >= flippedTime)
                    {
                        flippedTime = TimeElapsed + 4f;
                        isFlipped = !isFlipped;

                        if (isFlipped)
                        {
                            speed = 4f;
                            anim.SetTrigger("MeleeFight");
                        }
                        else
                        {
                            speed = 5f;
                            anim.SetTrigger("MeleeFight");
                        }
                    }


                    if (distance > 1f)
                    {
 
                        rb.position = Vector2.MoveTowards(rb.position, playerRb.position, speed * Time.deltaTime); //Move Enemy to Player
                        //var dir = playerRb.transform.position - rb.transform.position; //Calculate Player and Enemy direction
                        //transform.up = Vector2.Lerp(-transform.forward, dir, Time.deltaTime * 3f); //Rotate towards player
                        
                        // var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }

                    break;
                case UpgradeEnemy.UpgradeTwo:

                    //Enemy is not moving
                    //Enemy Delays (Charge)
                    
                    speed = 0;
                    
                    if (canCharge)
                    {
                        //targetTransparency = 1f;
                        anim.SetTrigger("MeleeFight");
                        StartCoroutine(ChargeAttack());
                        anim.SetTrigger("MeleeFight");
                    }
                    else
                    {
                        //targetTransparency = 0f;
                    }
                    
                    
                    if (transparency < targetTransparency)
                    {
                        transparency += .8f;
                    }
                    else if (transparency > targetTransparency && transparency >= 0f)
                    {
                        transparency -= Time.deltaTime;
                    }
                    
                    mat = GetComponent<Renderer>().material;
                    mat.SetFloat("_NegativeAmount", transparency);

                    
                    break;
                default:
                    
                    upgradeTypes = UpgradeEnemy.Base;
                    break;
            }
        }

    }
    
    [SerializeField] private Transform saveCurrentPlayerPosition; //Enemy is keeps tracking player position
    
    IEnumerator ChargeAttack()
    {
        canCharge = false;
        yield return new WaitForSeconds(5f); //Enemy is charging
        Transform saveCurrentPlayerPosition; //Enemy is keeps tracking player position
        saveCurrentPlayerPosition = playerRb.transform;    //Enemy saves player position
        rb.AddForce(new Vector2(saveCurrentPlayerPosition.position.x - rb.position.x, saveCurrentPlayerPosition.position.y - rb.position.y).normalized * 15f, ForceMode2D.Impulse);  //Enemy dash to the saves player location
        speed = 0; //After dashing, set speed back to 0
        yield return new WaitForSeconds(1f); //Cooling Down for 1 second
        rb.velocity = Vector2.zero;
        canCharge = true; //Cooling down complete, set can charge back to true
    }
    
 
    #endregion

    #region Ranged AI Functions

    public void RunFromPlayerEnemyMovement()
    {
        float distance = Vector2.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        var upgradeTypes = currentUpgradeEnemy;
        speed = 10f * Time.deltaTime;
        isFlanking = false;

        switch (upgradeTypes)
        {
            case UpgradeEnemy.Base:

                if(distance < 5f)
                {
                    var enemyPosition = rb.position;
                    var playerPosition = playerRb.position;
                    var dist = (enemyPosition - playerPosition).magnitude;
                    var mapped = Mathf.InverseLerp(10, 5, dist);
                    rb.position = Vector2.MoveTowards(enemyPosition, playerPosition, -mapped * speed);
                    rb.GetComponent<Animator>().SetTrigger("run");
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

                    Vector2 target = new Vector2(Mathf.Sin(cursor), Mathf.Cos(cursor)).normalized * 5f + playerRb.position;
                    rb.position = Vector2.MoveTowards(rb.position, target, 3f * Time.deltaTime);
                    rb.GetComponent<Animator>().SetTrigger("run");
                }
                else
                {
                    _centre = playerRb.position;

                    _angle += 2.4f * Time.deltaTime;
                    var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * 2f;
                    rb.position = _centre + offset;
                    isFlanking = false;
                    rb.GetComponent<Animator>().SetTrigger("run");
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
                upgradeTypes = UpgradeEnemy.Base;
                break;
        }
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

    
    #endregion
    
  
    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.PlayerTakeDamage, CancelTeleport);

        switch (currentUpgradeEnemy)
        {
            case UpgradeEnemy.Base:
                onNormalSpawn.Invoke();
                break;

            default:
                if (correctUpgradesToShowCrown.Contains(currentUpgradeEnemy))
                    onUpgradeSpawn.Invoke();
                break;
        }
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.PlayerTakeDamage, CancelTeleport);
    }
    
}