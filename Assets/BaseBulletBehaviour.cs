using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletBehaviour : MonoBehaviour
{
    [HideInInspector] public float timeToDie;
    private float lifeTime;
    public AnimationCurve velocityOverTime;
    public AnimationCurve sizeOverTime;

    private Vector3 iniSize;
    private Vector3 iniVel;
    private Rigidbody2D thisBody;
    private void Awake()
    {
        iniSize = transform.localScale;
        thisBody = GetComponent<Rigidbody2D>();
    }

    public void Initialise(float _lifeTime, AnimationCurve _velOverTime, AnimationCurve _sizeOverTime, Vector3 _iniVel)
    {
        //Lifetime
        timeToDie = Time.time + _lifeTime;
        lifeTime = _lifeTime;

        //Ini Properties
        transform.localScale = iniSize;
        velocityOverTime = _velOverTime;
        sizeOverTime = _sizeOverTime;
        iniVel = _iniVel;
    }
    void Update()
    {
        if (Time.time < timeToDie)
            EvaluateProperties();

        else gameObject.SetActive(false);
    }

    public void EvaluateProperties()
    {
        float _currentValue = 1-((timeToDie - Time.time) / lifeTime);
        //Debug.Log(_currentValue);

        thisBody.velocity = iniVel * velocityOverTime.Evaluate(_currentValue);
        transform.localScale = iniSize * sizeOverTime.Evaluate(_currentValue);
    }
}
