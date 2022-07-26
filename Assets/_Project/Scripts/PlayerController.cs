using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    public Camera sceneCamera;
    public Rigidbody2D rb;
    public InputReader InputReader;

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
        Animate();
    }

    private void Animate()
    {
        if (input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {  
        Move();
    }
    
    

    public float forceAmount;
    public bool isMoving;
    private void HandleInput()
    {

        mousePos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        desiredAimDir = mousePos - rb.position;

        input = InputReader.move;
        input.Normalize();


        Debug.DrawRay(transform.position, desiredAimDir);

    }

    public Vector2 input;

    void Move()
    {
        rb.velocity = new Vector2(input.x * runSpeed, input.y * runSpeed);

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
