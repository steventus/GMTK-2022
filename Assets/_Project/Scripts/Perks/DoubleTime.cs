using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTime : Perk
{
    public List<BulletData> BulletDatas = new List<BulletData>();
    
    public float AdditionalSpeed = 5f;
    private float oldSpeed;


    public override void RunPerk()
    {
        usedPerk = true;
        
        oldSpeed = AdditionalSpeed;
        foreach (var bullet in BulletDatas)
        {
            bullet.bulletSpeed += AdditionalSpeed;
        }
    }

    public override void ResetPerks()
    {
        foreach (var bullet in BulletDatas)
        {
            bullet.bulletSpeed = AdditionalSpeed;
        }

        usedPerk = false;
    }
}
