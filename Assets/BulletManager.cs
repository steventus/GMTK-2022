using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BulletManager : MonoBehaviour
{
    public bool isTemporary;
    public bool isTemporaryReady;

    public SpriteRenderer gunSprite;

    //Fire Properties
    public List<WeaponData> desiredWeapon;
    protected WeaponData curWeapon;
    private WeaponData oldWeapon;

    [SerializeField] private GameObject templateBullet;

    //Ammo
    public int maxAmmo;
    private int curMaxAmmo;
    private int curAmmo;
    public int ammoRegenPerSec;
    private bool isReloading;

    [Space]
    //Events
    public UnityEvent<AudioClip> onOutOfAmmo, onFire, onRegenAmmo, onFinishRegenAmmo;

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
        curMaxAmmo = maxAmmo;
    }

    public void InitialiseWeapon()
    {
        if (desiredWeapon.Count == 0)
        {
            ifCanFire = false;
            return;     
        }

        //Select weapon randomly from all possible weapons except from previous weapon of this bullet manager
        List<WeaponData> excludedLists = new List<WeaponData>();
        
        for (int i = 0; i < desiredWeapon.Count; i++)
        {
            if (desiredWeapon[i] != oldWeapon)
                excludedLists.Add(desiredWeapon[i]);
        }

        curWeapon = desiredWeapon[Random.Range(0, desiredWeapon.Count)];
        
        // Set weapon Sprite
        if(curWeapon.gunSprite != null)
        {
            gunSprite.sprite = curWeapon.gunSprite;
        }

        //Debug.Log("Weapon: " + curWeapon.name);

        //Initialise stats from SO
        fireRate = curWeapon.fireRate;
        bulletSpeed = curWeapon.bulletData.bulletSpeed;
        radius = 1;
        delayBetweenCycleInSec = 0;

        numberOfTimesToFirePerCycle = curWeapon.numOfBulletsInBurst;
        numberOfCycles = 1;
        angleBetweenEachBulletInCycle = curWeapon.angleBetweenBullet;
        delayBetweenEachRapidFireInSec = 0;
        curFireCost = curWeapon.costPerShot;

        //Update and Initialise Bullet
        oldWeapon = curWeapon;
        InitialiseBullet();
    }

    private void InitialiseBullet()
    {
        BulletData _selectedBullet = curWeapon.bulletData;
        
        Debug.Log("Bullet: " + _selectedBullet.name);

        foreach (GameObject _bullet in availableBullets)
        {
            bulletSpeed = _selectedBullet.bulletSpeed;

            _bullet.GetComponentInChildren<SpriteRenderer>().sprite = _selectedBullet.bulletSprite;
            _bullet.GetComponent<BaseBulletBehaviour>().Initialise(curWeapon.bulletData.bulletLifeTime, curWeapon.bulletData.velocityOverLifetime, curWeapon.bulletData.sizeOverLifetime, _bullet.transform.up * bulletSpeed);
        }

    }

    public void SelectWeapon()
    {
        InitialiseWeapon();
    }
    protected virtual void Update()
    {
        //UPDATE MOUSE POSITION

        if (Input.GetKeyDown(KeyCode.P))
            SlotMachine();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!CheckAmmo(curFireCost))
                return;

            Debug.Log("Fire");
            
            InterruptRegen();
            CallFire();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !isReloading)
        {
            InterruptRegen();

            coro_bulletRegen = StartCoroutine(BeginRegen());
        }
    }
    public virtual void CallFire()
    {
        //FIRE
        if (Time.time >= timeLastFired + (1 / fireRate) && !firing && ifCanFire)
        {
            if (isTemporary && !isTemporaryReady)
                return;

            timeLastFired = Time.time;
            firing = true;

            if (!isTemporary)
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
        #region Instantiate Bullet
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
        Vector3 targetdirection = GetComponent<PlayerController>().desiredAimDir;
        targetdirection.z = -20;
        _bullet.transform.rotation = Quaternion.LookRotation(targetdirection, ifFireFromBack ? -Vector3.forward : Vector3.forward);

        //ALTER ROTATION 
        _bullet.transform.Rotate(0, 0, _rotation);

        //INITIALISE BULLET
        _bullet.GetComponentInChildren<SpriteRenderer>().sprite = curWeapon.bulletData.bulletSprite;
        _bullet.GetComponent<BaseBulletBehaviour>().Initialise(curWeapon.bulletData.bulletLifeTime, curWeapon.bulletData.velocityOverLifetime, curWeapon.bulletData.sizeOverLifetime, _bullet.transform.up * bulletSpeed);
        _bullet.GetComponent<DamgerBullet>().BulletData = curWeapon.bulletData;

        #endregion


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

        //EVENTS
        //Choose sound clip based on gun - choose from resource

        //Invoke fire based on gun
        onFire.Invoke(curWeapon.soundOnFire);

    }
    protected bool CheckAmmo(int _cost)
    {
        if (curAmmo >= _cost) return true;
        else 
        {
            onOutOfAmmo.Invoke(curWeapon.soundOnOutOfAmmo);
            return false;
        }
        
    }
    protected void UpdateAmmo(int _cost)
    {
        curAmmo -= _cost;
        FindObjectOfType<UiPlayerAmmo>().SetPlayerAmmo(curAmmo);
    }
    IEnumerator BeginRegen()
    {
        if (isReloading) yield break;

        isReloading = true;
        yield return new WaitForSeconds(1);
            
        onRegenAmmo.Invoke(curWeapon.soundOnReload);

        while (curAmmo < curMaxAmmo)
        {
            yield return new WaitForSeconds(1);
            int _amountToRegen = (ammoRegenPerSec + Mathf.CeilToInt(PlayerUpgrades.numRegenUp * GetComponent<PlayerUpgrades>().regenAmmoPerSecUpgrade));
            
            //Debug
            Debug.Log(_amountToRegen);

            curAmmo += _amountToRegen;
            FindObjectOfType<UiPlayerAmmo>().SetPlayerAmmo(curAmmo);
            
            if (curAmmo >= curMaxAmmo)
            {
                isReloading = false;
                curAmmo = curMaxAmmo;
                FindObjectOfType<UiPlayerAmmo>().SetPlayerAmmo(curAmmo);

                onFinishRegenAmmo.Invoke(curWeapon.soundOnFinishReload);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private void InterruptRegen()
    {
        if (coro_bulletRegen != null)
        {
            //Debug.Log("interuppted");
            StopCoroutine(coro_bulletRegen);
            onFinishRegenAmmo.Invoke(curWeapon.soundOnFinishReload);
            coro_bulletRegen = null;
            isReloading = false;
        }
    }
    public void PlayOnlyOnce(AudioClip _clip)
    {
        AudioSource _source = GetComponentInChildren<AudioSource>();

        if (_source.isPlaying)
        {
            _source.Stop();
            return;
        }

        else
        {
            _source.clip = _clip;
            _source.loop = false;
            _source.Play();
        }
    }
    public void UpdateMaxAmmo()
    {
        curMaxAmmo = maxAmmo + (Mathf.CeilToInt(PlayerUpgrades.numAmmoUp * GetComponent<PlayerUpgrades>().maxAmmoUpgrade));
    }
    public void SlotMachine()
    {
        #region Initialisation and Remove Upgrades
        //Remove any temporary bullet managers (remove upgrades)
        foreach (BulletManager _manager in GetComponents<BulletManager>())
            if (_manager.isTemporary)
            {
                _manager.isTemporaryReady = false;
            }
        #endregion

        float _RNG = Random.Range(0, 101);

        //Upgrade Stat
        if (_RNG >= 0 && _RNG < 66)
        {
            int _RNGint = Random.Range(0, 3);
            switch (_RNGint)
            {
                case 0:
                    GetComponent<PlayerUpgrades>().IncreaseUpgrade(PlayerUpgrades.PlayerUpgradeType.movespeedUp);
                    break;

                case 1:
                    GetComponent<PlayerUpgrades>().IncreaseUpgrade(PlayerUpgrades.PlayerUpgradeType.maxAmmoUp);
                    break;

                case 2:
                    GetComponent<PlayerUpgrades>().IncreaseUpgrade(PlayerUpgrades.PlayerUpgradeType.regenAmmoUp);
                    break;
            }
            //Debug.Log("Common");
            //Debug.Log("numMove: " + PlayerUpgrades.numMoveSpeedUp);
            //Debug.Log("numAmmo: " + PlayerUpgrades.numAmmoUp);
            //Debug.Log("numRegen: " + PlayerUpgrades.numRegenUp);
        }
        //New Gun
        else if (_RNG >= 66 && _RNG < 88)
        {
            InitialiseWeapon();
            //Debug.Log("Uncommon");

        }
        //Critical Roll
        else
        {
            InitialiseWeapon();

            foreach (BulletManager _manager in GetComponents<BulletManager>())
                if (_manager.isTemporary)
                {
                    _manager.isTemporaryReady = true;
                    _manager.InitialiseWeapon();
                }
        }
    }
}
