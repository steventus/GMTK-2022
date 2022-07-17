using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimMouse : MonoBehaviour
{

    public Camera sceneCamera;
    public Transform aimCursor;
    public Transform gun;

    public SpriteRenderer gunSprite;
    private Vector2 moveDir;

    private Vector2 mousePos;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Update ()
    {
        HandleInput();
        UpdateAimCursor(aimCursor);
        UpdateWeaponAim(gun);
    }
    
    private void HandleInput()
    {
        mousePos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    }



    private void UpdateAimCursor(Transform parent)
    {
        var aimDir = (mousePos - (Vector2)parent.position).normalized;
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        parent.eulerAngles = new Vector3(0, 0, aimAngle);

    }
    
    private void UpdateWeaponAim(Transform parent)
    {
        var aimDir = (mousePos - (Vector2)parent.position).normalized;
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        parent.eulerAngles = new Vector3(0, 0, aimAngle);

        if (aimDir.x < 0)
        {
            gunSprite.flipY = true;
        }
        else if(aimDir.x > 0)
        {
            gunSprite.flipY = false;
        }


    }
}
