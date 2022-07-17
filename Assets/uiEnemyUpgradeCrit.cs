using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiEnemyUpgradeCrit : MonoBehaviour
{
    public Image thisImage;

    private void Awake()
    {
        thisImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        Messenger < bool>.AddListener(UiEvent.enemy_upgradeChangeCrit, setImage);
    }
    private void OnDisable()
    {
        Messenger<bool>.RemoveListener(UiEvent.enemy_upgradeChangeCrit, setImage);

    }
    public void setImage(bool _state)
    {
        GetComponent<Image>().enabled = _state;
    }
}
