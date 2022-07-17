using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : BulletManager
{
    public GameObject player;

    protected override void Update()
    {
        CallFire();
    }
    public override void CallFire()
    {
        //FIRE
        if (Time.time >= timeLastFired + (1 / fireRate) && !firing && ifCanFire && CheckAmmo(curFireCost))
        {
            timeLastFired = Time.time;
            firing = true;
            StartCoroutine(FireCycle(numberOfTimesToFirePerCycle));
        }
    }
    protected override void Fire(float _rotation)
    {
        GameObject _bullet = null;
        if (availableBullets[0] != null)
        {
            _bullet = availableBullets[0];
            if (_bullet.activeInHierarchy)
                Debug.Log("Insufficient number of bullets for this manager: " + gameObject.name + ". Bullet name: " + _bullet.name);
        }

        else
        {
            Debug.Log("Error: No bullet found.");
            return;
        }

        //"INSTANTIATE"
        _bullet.transform.position = transform.position;
        _bullet.transform.rotation = Quaternion.identity;
        _bullet.SetActive(true);

        //Update original object pooling list
        MoveBulletToBackOfList(_bullet);

        player = FindObjectOfType<PlayerController>().gameObject;

        //ROTATE TO PLAYER
        Vector3 targetdirection = player.transform.position - transform.position;
        targetdirection.z = -20;
        _bullet.transform.rotation = Quaternion.LookRotation(targetdirection, ifFireFromBack ? -Vector3.forward : Vector3.forward);

        //ALTER ROTATION 
        _bullet.transform.Rotate(0, 0, _rotation);

        //INITIALISE BULLET
        _bullet.GetComponentInChildren<SpriteRenderer>().sprite = curWeapon.bulletData.bulletSprite;
        _bullet.GetComponent<BaseBulletBehaviour>().Initialise(curWeapon.bulletData.bulletLifeTime, curWeapon.bulletData.velocityOverLifetime, curWeapon.bulletData.sizeOverLifetime, _bullet.transform.up * bulletSpeed);
        _bullet.GetComponent<DamgerBullet>().BulletData = curWeapon.bulletData;

    }
}
