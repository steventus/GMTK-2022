using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    //Weapon Stats
    public float fireRate;
    public int costPerShot;

    //Weapon Properties
    public int numOfBulletsInBurst;
    public float angleBetweenBullet;

    public BulletData bulletData;

    [Space]
    //Audio
    public AudioClip soundOnFire;
    public AudioClip soundOnReload, soundOnFinishReload, soundOnOutOfAmmo;
}
