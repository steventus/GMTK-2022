using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perk
    : MonoBehaviour
{
    public bool usedPerk;
    public abstract void RunPerk();

    public abstract void ResetPerks();
}
