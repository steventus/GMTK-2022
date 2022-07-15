using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    //Weapon Stats
    public float fireRate;
    public float costPerShot;

    //Weapon Properties
    public int numOfBulletsInBurst;
    public float angleBetweenBullet;

    
}