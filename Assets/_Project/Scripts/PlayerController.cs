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
    private float curRunSpeed;

    [HideInInspector] public Vector2 desiredAimDir;
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        curRunSpeed = runSpeed;
    }

    void Update ()
    {
        HandleInput();
    }
    
    private void FixedUpdate()
    {  
        Move();
    }

    public float forceAmount;
    private void HandleInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY);
        mousePos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        
      
        if (Input.GetMouseButtonDown(0))
        {
            Knockback(forceAmount);
        }
    }

    public void Knockback(float amount)
    {
        var direction =  mousePos - rb.position;
        rb.AddForce(-direction * amount, ForceMode2D.Force);
    }
    void Move()
    {
        rb.velocity = new Vector2(moveX * runSpeed, moveY * runSpeed);

        desiredAimDir = mousePos - rb.position;
        float aimAngle = Mathf.Atan2(desiredAimDir.y, desiredAimDir.x) * Mathf.Rad2Deg - 90f;

        Debug.DrawRay(transform.position, desiredAimDir);
       
    }

    public void Reset()
    {
        curRunSpeed = runSpeed;
    }

    public void Upgrade()
    {
        curRunSpeed = Mathf.Pow(runSpeed, PlayerUpgrades.numMoveSpeedUp * GetComponent<PlayerUpgrades>().moveSpeedUpgrade);
        runSpeed = curRunSpeed;
    }

}