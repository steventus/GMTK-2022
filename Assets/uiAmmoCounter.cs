using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiAmmoCounter : MonoBehaviour
{
    public Sprite full, midhigh, mid, low, empty;
    public UiPlayerAmmo ammo;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 _pos = FindObjectOfType<PlayerController>().transform.position;

        if (ammo.GetComponent<Slider>().value > 40)
        {
            SetImage(full);
        }

        else if (ammo.GetComponent<Slider>().value > 30)
        {
            SetImage(midhigh);
        }
        else if (ammo.GetComponent<Slider>().value > 20)
        {
            SetImage(mid);
        }
        else if (ammo.GetComponent<Slider>().value > 5)
        {
            SetImage(low);
        }
        else
        {
            SetImage(empty);
        }
        transform.position = new Vector3(_pos.x, _pos.y + 1.5f, _pos.z);
        
    }

    public void SetImage(Sprite _sprite)
    {
        GetComponent<Image>().sprite = _sprite;
    }
}
