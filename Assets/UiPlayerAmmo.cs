using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerAmmo : MonoBehaviour
{
    private void Start()
    {
        Initialise();
    }
    public void Initialise()
    {
        GetComponent<Slider>().value = GetComponent<Slider>().maxValue = 50; 
    }

    public void SetPlayerAmmo(float curAmmo)
    {
        GetComponent<Slider>().value = curAmmo;
    }
}
