using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SugarRush : Perk
{

    public PlayerController PlayerController;

    public override void RunPerk()
    {
        usedPerk = true;
        PlayerController.runSpeed = 8;
    }

    public override void ResetPerks()
    {
        usedPerk = false;
    }
}
