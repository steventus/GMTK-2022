using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBehaviour : MonoBehaviour
{

    private Image thisImage;
    private CanvasGroup thisGroup;

    public void Awake()
    {
        if (TryGetComponent(out Image _image))
            thisImage = _image;

        if (TryGetComponent(out CanvasGroup _group))
            thisGroup = _group;
    }
    public void FadeOut()
    {
        thisImage?.DOFade(0, 2);
        thisGroup?.DOFade(0, 2);

        if (thisGroup != null)
            thisGroup.blocksRaycasts = false;
    }

    public void FadeIn()
    {
        thisImage?.DOFade(1, 2);
        thisGroup?.DOFade(1, 2);

        if (thisGroup != null)
            thisGroup.blocksRaycasts = true;
    }
}
