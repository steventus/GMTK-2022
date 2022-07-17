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
        ArenaPerk.Randomize();
        ArenaPerk.Randomize();

        hasUsed = true;
    }

    public override void ResetPerks()
    {
        hasUsed = false;
    }
}
