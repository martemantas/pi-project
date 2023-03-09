using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movementSpeed;   //greitis 
    public float smoothingTime; //per kiek laiko sustos nuo mygtumo atleidimo

    private Vector2 movementInput;
    private Vector2 smoothedMovementInput;
    private Vector2 movementInputSmoothVelocity;

    public float dashSpeed;            //dash'o greitis
    public float dashLength;    //kaip ilgai dashina
    public float dashCooldown;    //dash'o cooldown
    private bool isWalking = false;

    private float activeMovementSpeed;
    public bool isDashing = false;
    private float dashCounter;
    private float dashCooldownCounter;
    


    SpriteRenderer spriteRenderer; //character sprite -M

    public Vector2 getMovementInput() {
        return movementInput;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        activeMovementSpeed = movementSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>(); // -M
      

    }

    void Update()
    {
        Dash();

        if (AudioManager.WalkingKeysPressed() && rb.velocity.magnitude > 0 )
        {
            FindObjectOfType<AudioManager>().PlayOnlyOnce("PlayerWalk");
        }
        if (!isWalking && FindObjectOfType<AudioManager>().GetSound("PlayerWalk").isPlaying)
        {
            FindObjectOfType<AudioManager>().Stop("PlayerWalk");
        }

    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            GetMovementDirection();
        }

        rb.velocity = smoothedMovementInput * activeMovementSpeed;
        if (rb.velocity != Vector2.zero)
        {
            isWalking = true;
            
        }
        else
            isWalking = false;


        // character pasisuka pagal vaiksciojimo krypti -M
        if (movementInput.x < 0 )
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        // -M
    }


    private void GetMovementDirection()
    {
        
        //gets inputs as a vector and normalize it so that 
        //the player would move diagonally, horizontally and vertically at the same speed
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput.Normalize();

        //makes it so that you wouldn't stop immediatly
        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput,
                                                    movementInput,
                                                    ref movementInputSmoothVelocity,
                                                    smoothingTime);
        
    }

    private void Dash()
    {
        //if "SPACE" is pressed changes speed (starts dash) 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCooldownCounter <= 0 && dashCounter <= 0)
            {
                
                activeMovementSpeed = dashSpeed;
                dashCounter = dashLength;
                isDashing = true;
                FindObjectOfType<AudioManager>().Play("PlayerDash");
            }
        }
        //counts how long the dash speed should last
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMovementSpeed = movementSpeed;
                dashCooldownCounter = dashCooldown;
                isDashing = false;
                
            }
        }
        //counts down dash cooldown
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
        
    }
}
