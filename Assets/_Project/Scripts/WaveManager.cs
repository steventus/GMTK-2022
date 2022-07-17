using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Wave
{
    public int enemiesNum;
    public GameObject[] Enemytype;
    public float timeBetweenEnemeySpawn;
}

public class WaveManager : MonoBehaviour
{

    public ArenaPerk ArenaPerk;
    public BulletManager BulletManager;
    public EnemyAIManager enemyAIManager;
    public int RoundNum;

    public ValueHandler ValueHandler;
    
    public List<Wave> waves;
    public Transform[] spawnPoints;

    private Wave currentWave;
    public int currentWaveNumber;
    private float nextSpawnTime;

    private bool canSpawn = true;
    private bool TrySpawn = false;

    //Critical Roll
    [SerializeField] private bool CriticalRole;
    [SerializeField] float enemyHealthUpgrade;
    [SerializeField] float enemyFireRateUpgrade;
    [SerializeField] int numOfBulletsUpgrade;

    public enum RNG_Upgrade { Common, Uncommon, Critical }

    public RNG_Upgrade rng_upgrade;

    private bool ShouldStop;
    private int myRoundNumber;

    private GameObject enemyGameObject;

    public UnityEvent onRoundStart, onRoundEnd;

    private void Start()
    {
        CriticalRole = false;
        enemyGameObject = GameObject.FindGameObjectWithTag("Fake");
    }

    private void Update()
    {
        if (ShouldStop) return;
        if (ValueHandler.currentRoundNum != RoundNum) return;
        
        currentWave = waves[currentWaveNumber];
        if (canSpawn && nextSpawnTime < Time.time)
        {
            SpawnWave();
        }

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Fake");

        if (totalEnemies.Length != 0) return;
        if (currentWaveNumber + 1 != waves.Count)
        {
            if (!TrySpawn) return;
            
            
            SpawnNextWave();

            if (ValueHandler.currentRoundNum > ValueHandler._waveManager.Length + 1)
            {
                Debug.Log("Stop heeeeeeeeeeeeeeeeeeeeeeeeee");
                ShouldStop = true;
            }
            return;
        }

        //Slotmachine anim
        FindObjectOfType<SlotMachine>().GetComponent<Animator>().Play("slotMachine_flyIn");

        onRoundEnd.Invoke();
    }
    public void RandomizeSlotMachine()
    {
        //Select new Weapon or just upgrade player properties
        //Check for Critical roll
        BulletManager.SlotMachine();

        if (CriticalRole)
        {
          
        }


        //Select enemy AI upgrade or just upgrade enemy properties
        //Check for Critical roll


        //Select new perk - Karim
        ArenaPerk.Perks.ForEach((perk => perk.ResetPerks()));
        ArenaPerk.Randomize();
    }
    public void StartNextRound()
    {
        RandomizeSlotMachine();
        ValueHandler.currentRoundNum++;
        onRoundStart.Invoke();
    }
    private bool isUsed;
    private bool GetValidate()
    {
        if (waves.Count == 1 && isUsed == false)
        {
            isUsed = true;
            return currentWaveNumber != waves.Count;
        }

        return currentWaveNumber + 1 != waves.Count;
    }
    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }
    void SpawnWave()
    {
        SpawnEnemyAtRandomPos();
        GetNextWave();

        if (currentWave.enemiesNum != 0) return;
        canSpawn = false;
        TrySpawn = true;

    }
    private void GetNextWave()
    {
        currentWave.enemiesNum--;
        nextSpawnTime = Time.time + currentWave.timeBetweenEnemeySpawn;
    }
    private void SpawnEnemyAtRandomPos()
    {
        GameObject randomEnemy = currentWave.Enemytype[Random.Range(0, currentWave.Enemytype.Length)];
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Apply upgrades
        //Enemy Property upgrades
        /*   randomEnemy.GetComponent<EnemyAIManager>().enemyHealth += 100;
           randomEnemy.GetComponent<EnemyMovement>().speed += 5;*/

        //AI upgrades
        //if is critical roll
        //else (Random.Range(0,101) >= 90) run code to apply upgrade
        bool _ifCriticalRoll = false;

        switch (_ifCriticalRoll)
        {
            case true:
                break;

            case false:
                //Check unlocked upgrades

                //Select one

                //Apply upgrade
                break;
        }






        Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
    }

   
}