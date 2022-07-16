using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalArena : Perk
{
    public ArenaPerk ArenaPerk;
    public override void RunPerk()
    {
        ArenaPerk.Randomize();
        ArenaPerk.Randomize();
    }

    public override void ResetPerks()
    {
    }
}
