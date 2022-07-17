using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class arenaUi : MonoBehaviour
{
    private void OnEnable()
    {
        Messenger<Sprite>.AddListener(UiEvent.arena_perkChange, changeImage);
    }
    private void OnDisable()
    {
        Messenger<Sprite>.RemoveListener(UiEvent.arena_perkChange, changeImage);
    }
    public void changeImage(Sprite _image)
    {
        GetComponent<Image>().sprite = _image;
    }
}
