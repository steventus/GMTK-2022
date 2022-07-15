using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    public Camera sceneCamera;
    Rigidbody2D rb;

    float moveX;
    float moveY;

    private Vector2 moveDir;
    private Vector2 mousePos;
    
    public float runSpeed = 20.0f;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update ()
    {
        HandleInput();
    }
    
    private void FixedUpdate()
    {  
        Move();
    }

    
    private void HandleInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY);
        mousePos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void Move()
    {
        rb.velocity = new Vector2(moveX * runSpeed, moveY * runSpeed);

        Vector2 aimDirection = mousePos - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
