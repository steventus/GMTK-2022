using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct RegainHealthSystem
{
    public int requiredKill;
    public int regainHealthAmount;
}

[CreateAssetMenu]
public class RegainSystemSO : ScriptableObject
{
    public List<RegainHealthSystem> RegainHealthSystems = new List<RegainHealthSystem>();

}
