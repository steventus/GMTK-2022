using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class UiWheel : MonoBehaviour
{
    public CanvasGroup thisGroup;
    public Image wheelImage;
    public Image flashImage;

    public UnityEvent wheelRolled, wheelCriticalRolled;

    public bool ifCrit;

    private void Awake()
    {
        DOTween.Init(false, false);
    }
    public void Initialise()
    {
        Sequence _ini = DOTween.Sequence();

        _ini.Append(thisGroup.DOFade(0, 0));
        _ini.Join(flashImage.DOFade(0, 0));
        _ini.Play();
    }

    public void ShowWheel(Sprite _selectedImage, bool _ifCrit)
    {
        //Ini
        wheelImage.sprite = _selectedImage;
        Sequence _ini = DOTween.Sequence();

        //Flash to white on the wheel
        _ini.Append(flashImage.DOFade(1, 0.2f));

        //Flash fade to image
        _ini.Append(flashImage.DOFade(0, 1));
        _ini.Join(thisGroup.DOFade(1, 0));

        switch (_ifCrit)
        {
            case false:
                wheelRolled.Invoke();
                break;

            case true:
                wheelCriticalRolled.Invoke();
                break;
        }
    }

    public void ShowWheelSpecial(bool _ifCrit)
    {
        Sequence _ini = DOTween.Sequence();

        //Flash to white on the wheel
        _ini.Append(flashImage.DOFade(1, 0.2f));

        //Flash fade to image
        _ini.Append(flashImage.DOFade(0, 1));
        _ini.Join(thisGroup.DOFade(1, 0));

        switch (_ifCrit)
        {
            case false:
                wheelRolled.Invoke();
                break;

            case true:
                wheelCriticalRolled.Invoke();
                break;
        }
    }

    public void RemoveImage()
    {
        Sequence _ini = DOTween.Sequence();

        //Flash fade to image
        _ini.Append(flashImage.DOFade(0, 3));
        _ini.Join(thisGroup.DOFade(0, 3));
    }

    public void SetImage(Sprite _image)
    {
        wheelImage.sprite = _image;
    }
}
