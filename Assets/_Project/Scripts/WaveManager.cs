using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Wave
{
    public GameObject[] Enemytype;
    public float timeBetweenEnemeySpawn;
}

public class WaveManager : MonoBehaviour
{

    public ArenaPerk ArenaPerk;
    public BulletManager BulletManager;
    public int RoundNum;

    public RoundHandler roundHandler;
    
    public List<Wave> waves;
    public Transform[] spawnPoints;

    private List<Wave> _infiniteWaves = new List<Wave>(3);

    private Wave currentWave;
    public int currentWaveNumber;
    private float nextSpawnTime;

    private bool canSpawn = true;
    private bool TrySpawn = false;

    //Critical Roll
    public static bool CriticalRoll;
    public EnemyMovement.UpgradeEnemy chosenUpgradeType;

    public enum RNG_Upgrade { Common, Uncommon, Critical }

    public RNG_Upgrade rng_upgrade;

    private bool ShouldStop;
    private int myRoundNumber;

    private GameObject enemyGameObject;

    private bool roundEnded = false;
    public UnityEvent onRoundStart, onRoundEnd;

    public int enemiesNum = 0;
    private void Start()
    {
        enemiesNum = Random.Range(roundHandler.EnemySpawnMin, roundHandler.EnemySpawnMax);
        CriticalRoll = false;
        enemyGameObject = GameObject.FindGameObjectWithTag("Fake");
    }

    private void Update()
    {
        if (ShouldStop) return;
        if (roundHandler.currentRoundNum != RoundNum) return;
        
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
            enemiesNum = Random.Range(roundHandler.EnemySpawnMin, roundHandler.EnemySpawnMax);

            SpawnNextWave();

            // if (roundHandler.currentRoundNum > roundHandler._waveManager.Length + 1)
            // {
            //     ShouldStop = true;
            // }
            return;
        }

        RoundEnded();
    }

    private void RoundEnded()
    {
        if (!roundEnded)
        {
            roundEnded = true;
            //Slotmachine anim
            FindObjectOfType<SlotMachine>().GetComponent<Animator>().Play("slotMachine_flyIn");

            onRoundEnd.Invoke();
            Messenger.Broadcast(GameEvent.OnRoundEnd);
            Debug.Log("OnRoundEnd");
        }
    }

    public void RandomizeSlotMachine()
    {
        //Select new Weapon or just upgrade player properties
        //Check for Critical roll
        BulletManager.SlotMachine();

        //Select enemy AI upgrade or just upgrade enemy properties
        //Check for Critical roll
        CriticalRoll = GetComponent<EnemyUpgradeManager>().SlotMachineIfCritical();
      
        //Select new perk - Karim
        ArenaPerk.Perks.ForEach((perk => perk.ResetPerks()));
        ArenaPerk.Randomize();
    }
    public void StartNextRound()
    {
        RandomizeSlotMachine();
        roundHandler.currentRoundNum++;
        
        roundHandler.EnemySpawnMin += 1;
        roundHandler.EnemySpawnMax += 1;
        
        // Randomize properties        
        RoundNum++;
        currentWaveNumber = 0;
        
        
        onRoundStart.Invoke();
        roundEnded = false;
        Messenger.Broadcast(GameEvent.SpawnNewRound);
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

        if (enemiesNum != 0) return;
        canSpawn = false;
        TrySpawn = true;

    }
    private void GetNextWave()
    {
        enemiesNum--;
        nextSpawnTime = Time.time + currentWave.timeBetweenEnemeySpawn;
    }
    private void SpawnEnemyAtRandomPos()
    {
        GameObject randomEnemy = currentWave.Enemytype[Random.Range(0, currentWave.Enemytype.Length)];
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Apply AI upgrades based on Critical Roll or not
        switch (CriticalRoll)
        {
            //If Critical Roll
            case true:
                randomEnemy.GetComponent<EnemyMovement>().currentUpgradeEnemy = chosenUpgradeType;
                break;

            //Else
            case false:
                //10%
                if (Random.Range(0, 101) >= 90)
                    randomEnemy.GetComponent<EnemyMovement>().currentUpgradeEnemy = chosenUpgradeType;
                break;
        }

        Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
    }
    public void UpdateEnemyUpgrade(int _choice)
    {
        switch (_choice)
        {
            case 0:
                chosenUpgradeType = EnemyMovement.UpgradeEnemy.UpgradeOne;
                break;

            case 1:
                chosenUpgradeType = EnemyMovement.UpgradeEnemy.UpgradeTwo;
                break;

            case 2:
                chosenUpgradeType = EnemyMovement.UpgradeEnemy.UpgradeThree;
                break;

            case 3:
                chosenUpgradeType = EnemyMovement.UpgradeEnemy.UpgradeFour;
                break;

            default:
                Debug.Log("Received invalid int _choice to apply upgrade");
                break;
        }
        Debug.Log("chosenUpgradeType: " + chosenUpgradeType);
    }

}