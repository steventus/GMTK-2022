using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gunUi : MonoBehaviour
{
    private void OnEnable()
    {
        Messenger<Sprite>.AddListener(UiEvent.player_gunChange, changeImage);
    }
    private void OnDisable()
    {
        Messenger<Sprite>.RemoveListener(UiEvent.player_gunChange, changeImage);
    }
    public void changeImage(Sprite _image)
    {
        Debug.Log("test");
        GetComponent<Image>().sprite = _image;
    }
}
