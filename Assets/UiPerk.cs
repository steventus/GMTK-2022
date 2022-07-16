using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiPerk : MonoBehaviour
{
    public TMP_Text thisText;
    public ArenaPerk thisPerkManager;

    private Perk oldPerk;

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        thisPerkManager = FindObjectOfType<ArenaPerk>();
    }
    void Update()
    {
        if (thisPerkManager.SelecetedPerk != oldPerk)
        {
            thisText.text = "Perk: " + thisPerkManager.SelecetedPerk.name;
            oldPerk = thisPerkManager.SelecetedPerk;
        }   
    }
}
