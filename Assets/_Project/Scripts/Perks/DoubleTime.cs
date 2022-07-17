using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTime : Perk
{
    public List<BulletData> BulletDatas = new List<BulletData>();
    
    public float additionalPerkSpeed = 4f;
    public float additionalCriticalSpeed = 10f;

    
    private float _newSpeed;


    public override void RunPerk()
    {
        usedPerk = true;
        var speed = isCritical ? additionalCriticalSpeed : additionalPerkSpeed;
        _newSpeed += speed;
        
        foreach (var bullet in BulletDatas)
        {
            
            bullet.bulletSpeed += speed;
        }
    }

    public override void ResetPerks()
    {
        foreach (var bullet in BulletDatas)
        {
            bullet.bulletSpeed -= _newSpeed;
        }

        usedPerk = false;
    }
}
