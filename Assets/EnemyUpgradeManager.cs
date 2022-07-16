using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpgradeManager : MonoBehaviour
{
    public float healthUpgrade;
    public static int numhealthUp;
    public enum EnemyUpgradeType
    {
        healthUp
    }

    public static List<EnemyMovement.UpgradeEnemy> unlockedUpgrades = new List<EnemyMovement.UpgradeEnemy>();

    public void IncreaseUpgrade(EnemyUpgradeType _type)
    {
        switch (_type)
        {
            case EnemyUpgradeType.healthUp:
                numhealthUp++;
                Debug.Log("enemy numHealthUp: " + numhealthUp);
                break;

        }
    }
    public void Reset()
    {
        numhealthUp = 0;
    }

    public bool SlotMachineIfCritical()
    {
        float _rng = Random.Range(0, 101);
        Debug.Log(_rng);
        //66%
        if (_rng >= 0 && _rng < 66)
        {
            IncreaseUpgrade(EnemyUpgradeType.healthUp);
            return false;
        }

        //22%
        else if (_rng >= 66 && _rng < 88)
        {
            if (!unlockedUpgrades.Contains(EnemyMovement.UpgradeEnemy.UpgradeOne))
                unlockedUpgrades.Add(EnemyMovement.UpgradeEnemy.UpgradeOne);
            return false;
        }

        //12%
        else
        {
            return true;
        }
    }

    public EnemyMovement.UpgradeEnemy findAvailableUpgrade()
    {
        if (unlockedUpgrades.Count == 0)
            return EnemyMovement.UpgradeEnemy.Base;

        else return unlockedUpgrades[Random.Range(0, unlockedUpgrades.Count)];
    }
}
