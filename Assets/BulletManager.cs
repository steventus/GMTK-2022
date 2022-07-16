using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BulletManager : MonoBehaviour
{

    //PlayerInput

    //Fire Properties
    public List<WeaponData> desiredWeapon;

    //Bullet
    public List<BulletData> desiredBullet;
    [SerializeField] private GameObject templateBullet;

    //Ammo
    public int maxAmmo;
    public int curAmmo;
    public int ammoRegenPerSec;

    #region 
    //Firing a cycle
    [HideInInspector] public float fireRate = 1, radius = 1, bulletSpeed = 5, delayBetweenCycleInSec = 0.1f;
    [HideInInspector] public int numberOfTimesToFirePerCycle, numberOfCycles = 1;
    [HideInInspector] public bool ifFireFromBack = false;
    [HideInInspector] protected int curFireCost;

    //Properties within each firing cycle
    [HideInInspector] public float angleBetweenEachBulletInCycle, delayBetweenEachRapidFireInSec;
    private int fireIndex, secondaryFireIndex;

    protected float timeLastFired;
    private float currentAngle;

    //Object pooling
    public int numberOfBulletsToSpawn;
    protected List<GameObject> availableBullets = new List<GameObject>();

    //Sound
    [HideInInspector] public AudioClip bulletFire, bulletDie;

    protected bool firing = false;
    public bool ifCanFire = true;

    private Coroutine coro_bulletRegen;
    #endregion
    private void Awake()
    {
        for (int i = 0; i < numberOfBulletsToSpawn; i++)
        {
            GameObject _spawned = Instantiate(templateBullet, transform.position, Quaternion.identity);
            availableBullets.Add(_spawned);
            _spawned.SetActive(false);
        }

        SelectWeapon();


        curAmmo = maxAmmo;
    }

    public void InitialiseWeapon()
    {
        if (desiredWeapon.Count == 0)
        {
            ifCanFire = false;
            return;     
        }

        WeaponData _selectedWep = desiredWeapon[Random.Range(0, desiredWeapon.Count)];
        Debug.Log("Weapon: " + _selectedWep.name);

        fireRate = _selectedWep.fireRate;
        radius = 1;
        delayBetweenCycleInSec = 0;

        numberOfTimesToFirePerCycle = _selectedWep.numOfBulletsInBurst;
        numberOfCycles = 1;
        angleBetweenEachBulletInCycle = _selectedWep.angleBetweenBullet;
        delayBetweenEachRapidFireInSec = 0;


        curFireCost = _selectedWep.costPerShot;
    }

    public void InitialiseBullet()
    {
        BulletData _selectedBullet = desiredBullet[Random.Range(0, desiredBullet.Count)];
        Debug.Log("Bullet: " + _selectedBullet.name);

        foreach (GameObject _bullet in availableBullets)
        {
            bulletSpeed = _selectedBullet.bulletSpeed;

            _bullet.GetComponent<SpriteRenderer>().sprite = _selectedBullet.bulletSprite;
        }

    }

    public void SelectWeapon()
    {
        InitialiseWeapon();
        InitialiseBullet();
    }

    protected virtual void Update()
    {
        //UPDATE MOUSE POSITION

        if (Input.GetKeyDown(KeyCode.P))
            SelectWeapon();

        if (Input.GetKey(KeyCode.Mouse0))
            CallFire();

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (coro_bulletRegen != null)
            {
                Debug.Log("interuppted");
                StopCoroutine(coro_bulletRegen);
            }

            coro_bulletRegen = StartCoroutine(BeginRegen());
        }
    }

    public virtual void CallFire()
    {
        //FIRE
        if (Time.time >= timeLastFired + (1 / fireRate) && !firing && ifCanFire && CheckAmmo(curFireCost))
        {
            timeLastFired = Time.time;
            firing = true;
            UpdateAmmo(curFireCost);
            StartCoroutine(FireCycle(numberOfTimesToFirePerCycle));
        }
    }
    protected void MoveBulletToBackOfList(GameObject _bullet)
    {
        if (availableBullets.Contains(_bullet))
        {
            //Move to back of list
            availableBullets.Add(_bullet);
            availableBullets.RemoveAt(0);
        }
    }
    protected virtual void Fire(float _rotation)
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
        Vector3 targetdirection = transform.up;
        targetdirection.z = -20;
        _bullet.transform.rotation = Quaternion.LookRotation(targetdirection, ifFireFromBack ? -Vector3.forward : Vector3.forward);

        //ALTER ROTATION 
        _bullet.transform.Rotate(0, 0, _rotation);

        _bullet.GetComponent<Rigidbody2D>().velocity = _bullet.transform.up * bulletSpeed;

        //INITIALISE BULLET
    }
    protected IEnumerator FireCycle(int _numberOfRapidFire)
    {
        bool _isEven = false;
        if (numberOfTimesToFirePerCycle % 2 == 0)
            _isEven = true;


        for (int i = 0; i < numberOfCycles; i++)
        {
            firing = true;
            currentAngle = -(angleBetweenEachBulletInCycle * (float)Mathf.Floor(numberOfTimesToFirePerCycle / 2));

            
            while (firing)
            {
                //skip center bullet if even
                if (currentAngle == 0 && _isEven)
                {
                }

                else
                {
                    Fire(currentAngle);

                    secondaryFireIndex++;
                    if (secondaryFireIndex >= numberOfTimesToFirePerCycle)
                    {
                        fireIndex = 0;
                        secondaryFireIndex = 0;
                        firing = false;
                        yield return null;
                    }
                }
                currentAngle += angleBetweenEachBulletInCycle;

                yield return new WaitForSeconds(delayBetweenEachRapidFireInSec);
            }
            yield return new WaitForSeconds(delayBetweenCycleInSec);
        }
    }

    protected bool CheckAmmo(int _cost)
    {
        if (curAmmo >= _cost) return true;
        else return false;
    }

    protected void UpdateAmmo(int _cost)
    {
        curAmmo -= _cost;
        FindObjectOfType<UiPlayerAmmo>().SetPlayerAmmo(curAmmo);
    }

    IEnumerator BeginRegen()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("begin regen");
        while (curAmmo < maxAmmo)
        {
            yield return new WaitForSeconds(1);
            curAmmo += ammoRegenPerSec;
            FindObjectOfType<UiPlayerAmmo>().SetPlayerAmmo(curAmmo);
            
            if (curAmmo >= maxAmmo)
            {
                curAmmo = maxAmmo;
                FindObjectOfType<UiPlayerAmmo>().SetPlayerAmmo(curAmmo);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
