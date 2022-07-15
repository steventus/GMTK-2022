using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class BulletData : ScriptableObject
{
    public float bulletSpeed;
    public UnityAction onSpawn, onHit;
    
    //Art
    public Sprite bulletSprite;
}
