using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerCriticalUi : MonoBehaviour
{
    private void OnEnable()
    {
        Messenger<bool>.AddListener(UiEvent.player_gunChangeCrit, setImage);
    }
    private void OnDisable()
    {
        Messenger<bool>.RemoveListener(UiEvent.player_gunChangeCrit, setImage);
    }
    public void setImage(bool _state)
    {
        GetComponent<Image>().enabled = _state;
    }
}
