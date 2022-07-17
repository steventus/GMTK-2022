using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private void Update()
    {
        if(Reference.cam != this)
            Reference.cam = this;
        
    }
    
    public void ShakeCamera(float strength = 1f, float duration = 3f){
        transform.DOShakePosition(duration, strength, 10, 90);
    }
}
