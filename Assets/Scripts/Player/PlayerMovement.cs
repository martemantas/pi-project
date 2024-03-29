using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour//, IDataPersistance
{
    public KeyCode dashKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

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

    public float activeMovementSpeed;
    public bool isDashing = false;
    private float dashCounter;
    private float dashCooldownCounter;

    private float tmpspeed = -1;
    private float tmpdash;

    private Animator animator; //for animations
    SpriteRenderer spriteRenderer; //character sprite -M

    public Vector2 getMovementInput() {
        return movementInput;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        activeMovementSpeed = movementSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>(); // -M
        animator = GetComponent<Animator>(); // -M

        dashKey = ControlManager.CM.dash;
        upKey = ControlManager.CM.up;
        downKey = ControlManager.CM.down;
        leftKey = ControlManager.CM.left;
        rightKey = ControlManager.CM.right;
    }

    /*public void LoadData(GameData data)
    {
        tmpspeed = data.stats[6];
        tmpdash = data.stats[8];

        movementSpeed = tmpspeed;
        activeMovementSpeed = tmpspeed;
        dashLength = tmpdash;
    }
    public void SaveData(ref GameData data)
    {
        data.stats[6] = movementSpeed;
        data.stats[8] = dashLength;
    }*/

    void Start()
    {
        SkillTree skills = FindObjectOfType<SkillTree>();
        movementSpeed = skills.SkillLevels[6] * 0.5f + 1;
        activeMovementSpeed = skills.SkillLevels[6] * 0.5f + 1;
        dashLength = skills.SkillLevels[8] * 0.5f + 0.15f;
        /*if (tmpspeed > -1)
        {
            movementSpeed = tmpspeed;
            activeMovementSpeed = tmpspeed;
            dashLength = tmpdash;
        }*/
    }

    void Update()
    {
        Dash();

        if (AudioManager.WalkingKeysPressed() && rb.velocity.magnitude > 0 )
        {
            FindObjectOfType<AudioManager>().PlayOnlyOnce("PlayerWalk");
            animator.SetFloat("lastX", movementInput.x);
            animator.SetFloat("lastY", movementInput.y);
            //MoneyManager.AddMoney(10);
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
            animator.SetFloat("horizontal", movementInput.x);
            animator.SetFloat("vertical", movementInput.y);
            animator.SetFloat("speed", movementInput.SqrMagnitude());
        }

        rb.velocity = smoothedMovementInput * activeMovementSpeed;
        if (rb.velocity != Vector2.zero)
        {
            isWalking = true;
            animator.SetBool("isWalking", true); // -M
            //animator.SetBool("isShooting", false); // -M

        }
        else
        {
            isWalking = false;
            animator.SetBool("isWalking", false); // -M
            //animator.SetBool("isShooting", false); // -M

        }



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
        if (Input.GetKey(leftKey))
            movementInput.x = -1;
        else if (Input.GetKey(rightKey))
            movementInput.x = 1;
        else
            movementInput.x = 0;

        if(Input.GetKey(upKey))
            movementInput.y = 1;
        else if(Input.GetKey(downKey))
            movementInput.y = -1;
        else
            movementInput.y = 0;

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
        if (Input.GetKeyDown(dashKey))
        {
            if (dashCooldownCounter <= 0 && dashCounter <= 0)
            {
                isDashing = true;
                animator.SetBool("dash", isDashing);
                activeMovementSpeed = dashSpeed;
                dashCounter = dashLength;
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
                animator.SetBool("dash", isDashing);
            }
        }
        //counts down dash cooldown
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
        
    }
}
