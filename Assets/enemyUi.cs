using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyUi : MonoBehaviour
{
    private void OnEnable()
    {
        Messenger<Sprite>.AddListener(UiEvent.enemy_upgradeChange, changeImage);
    }
    private void OnDisable()
    {
        Messenger<Sprite>.RemoveListener(UiEvent.enemy_upgradeChange, changeImage);
    }
    public void changeImage(Sprite _image)
    {
        GetComponent<Image>().sprite = _image;
    }
}
