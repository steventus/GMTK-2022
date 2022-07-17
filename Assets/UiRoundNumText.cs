using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiRoundNumText : MonoBehaviour
{
    public RoundHandler thishandler;
    void Start()
    {
        thishandler = FindObjectOfType<RoundHandler>();
    }

    void Update()
    {
        GetComponent<TMP_Text>().text = ""+thishandler.currentRoundNum;
    }
}
