using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : BulletManager
{
    public GameObject player;

    void Start()
    {

    }

    void Update()
    {
        CallFire();
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

        //ROTATE TO PLAYER
        Vector3 targetdirection = player.transform.position - transform.position;
        targetdirection.z = -20;
        _bullet.transform.rotation = Quaternion.LookRotation(targetdirection, ifFireFromBack ? -Vector3.forward : Vector3.forward);

        //ALTER ROTATION 
        _bullet.transform.Rotate(0, 0, _rotation);

        _bullet.GetComponent<Rigidbody2D>().velocity = _bullet.transform.up * bulletSpeed;

        //INITIALISE BULLET    

    }
}