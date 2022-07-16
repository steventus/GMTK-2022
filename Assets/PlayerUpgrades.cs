using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public float moveSpeedUpgrade, maxAmmoUpgrade, regenAmmoPerSecUpgrade;
    public static int numMoveSpeedUp, numAmmoUp, numRegenUp;

    public enum PlayerUpgradeType
    {
        movespeedUp,
        maxAmmoUp,
        regenAmmoUp
    }
    public void IncreaseUpgrade(PlayerUpgradeType _type)
    {
        switch (_type)
        {
            case PlayerUpgradeType.movespeedUp:
                numMoveSpeedUp++;
                break;

            case PlayerUpgradeType.maxAmmoUp:
                numAmmoUp++;
                break;

            case PlayerUpgradeType.regenAmmoUp:
                numRegenUp++;
                break;
        }
    }
    public void Reset()
    {
        numMoveSpeedUp = 0;
        numAmmoUp = 0;
        numRegenUp = 0;
    }
}
