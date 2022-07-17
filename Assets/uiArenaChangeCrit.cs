using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiArenaChangeCrit : MonoBehaviour
{
    public Image thisImage;

    private void Awake()
    {
        thisImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        Messenger<bool>.AddListener(UiEvent.arena_perkChangeCrit, setImage);
    }
    private void OnDisable()
    {
        Messenger<bool>.RemoveListener(UiEvent.arena_perkChangeCrit, setImage);

    }
    public void setImage(bool _state)
    {
        GetComponent<Image>().enabled = _state;
    }
}
