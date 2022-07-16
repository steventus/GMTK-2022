using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryBullletManager : BulletManager
{
    public override void CallFire()
    {
        if (Time.time >= timeLastFired + (1 / fireRate) && !firing && ifCanFire)
        {
            timeLastFired = Time.time;
            firing = true;
            StartCoroutine(FireCycle(numberOfTimesToFirePerCycle));
        }
    }
    public void DestroyThisManager()
    {
        Destroy(this);
    }
}
