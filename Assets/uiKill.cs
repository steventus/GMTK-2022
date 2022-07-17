using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class uiKill : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void OnEnable()
    {
        Messenger<int>.AddListener(UiEvent.player_kill, SetText);
    }
    private void OnDisable()
    {
        Messenger<int>.RemoveListener(UiEvent.player_kill, SetText);
    }
    public void SetText(int _num)
    {
        text.text = "KILLS " + _num;
    }
}
