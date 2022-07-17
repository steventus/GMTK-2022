using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalArena : Perk
{
    public ArenaPerk ArenaPerk;

    private bool hasUsed;
    public override void RunPerk()
    {
        if (hasUsed) return;
        hasUsed = true;
        
        isCritical = true;
        ArenaPerk.Randomize();
    }

    public override void ResetPerks()
    {
        hasUsed = false;
        isCritical = false;
    }
}
