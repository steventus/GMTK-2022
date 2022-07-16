using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleTime : Perk
{
    public List<BulletData> BulletDatas = new List<BulletData>();

    
    public float AdditionalSpeed = 5f;
    private float oldSpeed;


    public  bool UsedPerk { get; set; }

    public override void RunPerk()
    {
        oldSpeed = AdditionalSpeed;
        foreach (var bullet in BulletDatas)
        {
            bullet.bulletSpeed += AdditionalSpeed;
        }

        UsedPerk = true;
    }

    public override void ResetPerks()
    {
        foreach (var bullet in BulletDatas)
        {
            bullet.bulletSpeed = AdditionalSpeed;
        }

        UsedPerk = false;
    }
}
