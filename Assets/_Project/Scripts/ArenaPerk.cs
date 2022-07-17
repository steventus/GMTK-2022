using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaPerk : MonoBehaviour, ISlotMachine
{
    public List<Perk> Perks = new List<Perk>();

    public Perk SelecetedPerk;
    
    public void Randomize()
    {
        var UnUsedPerks = Perks.Where(x => !x.usedPerk).ToList();
        var random = Random.Range(0, UnUsedPerks.Count);

        SelecetedPerk = Perks[random]; 
        SelecetedPerk.RunPerk();
    }
}
