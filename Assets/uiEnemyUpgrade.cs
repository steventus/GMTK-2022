using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiEnemyUpgrade : MonoBehaviour
{
    public Image thisImage;
    private void OnEnable()
    {
        Messenger<Sprite>.AddListener(UiEvent.enemy_upgradeChange, SetImage);
    }
    private void OnDisable()
    {
        Messenger<Sprite>.RemoveListener(UiEvent.enemy_upgradeChange, SetImage);

    }
    public void SetImage(Sprite _image)
    {
        thisImage.sprite = _image;
    }
}
