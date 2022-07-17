using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoChange : Perk
{
    public bool UsedPerk { get; set; }

    public override void RunPerk()
    {
        UsedPerk = true;
    }

    public override void ResetPerks()
    {
        UsedPerk = false;
    }
    
    
}
