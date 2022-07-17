using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SugarRush : Perk
{

    public PlayerController PlayerController;
    public float criticalSpeed = 12;
    public float perkSpeed = 8;

    public override void RunPerk()
    {
        usedPerk = true;
        PlayerController.runSpeed = isCritical ? criticalSpeed : perkSpeed;
    }

    public override void ResetPerks()
    {
        usedPerk = false;
        PlayerController.runSpeed = 5;
    }
}
