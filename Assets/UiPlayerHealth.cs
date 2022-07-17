using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerHealth : MonoBehaviour
{
    public GameObject templateHealth;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        Messenger<int>.AddListener(UiEvent.player_takeDamage, SetImage);
    }
    private void OnDisable()
    {
        Messenger<int>.RemoveListener(UiEvent.player_takeDamage, SetImage);

    }
    public void CreateImage()
    {
        Instantiate(templateHealth, transform);
    }

    public void DestroyImage()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }
    public void SetImage(int _number)
    {
        for (int i = transform.childCount-1; i>= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < _number; i++)
        {
            CreateImage();
        }
    }
}
