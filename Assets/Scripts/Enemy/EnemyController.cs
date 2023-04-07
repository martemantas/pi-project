using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isAiActive = false;
    private AIDestinationSetter dest;
    public float speed;        //greitis
    public float attackRadius; //atakos range

    public LayerMask playerMask; //zaidejo layer
    public float scaler = 0.3f;

    public Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 direction;
    private Vector2 avoidVector;

    private bool isInAttackRange;
    public bool inRoom = false;

    SpriteRenderer spriteRenderer; //enemy sprite -M
    private Animator anim;

    // Enemy attack
    public float damage = 0.5f;
    public float attackPause;
    private float attackPauseCounter;
    private float rangePauseCounter;
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletDamage;

    private bool knockBackImunity = false;
    public float knockBackTime;
    private float knockBackCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // -M
        anim = GetComponent<Animator>();

        knockBackImunity = false;
        knockBackCounter = knockBackTime;
        if (knockBackTime <= 0)
            knockBackTime = 0.5f;
        dest = this.GetComponent<AIDestinationSetter>();
        dest.enabled = isAiActive;
    }

    private void Update()
    {
        if (inRoom)
        {
            if (!isAiActive && dest != null) {
                dest.target = target;
                isAiActive = true;
                dest.enabled = isAiActive;
            }
            isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, playerMask);
            
            direction = target.position - transform.position;
            direction.Normalize();
            movement = direction;
            if (knockBackImunity && knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                movement *= -1;
                Move(movement * (1 - scaler) + avoidVector * scaler);
            }
            if (knockBackCounter <= 0 && knockBackImunity)
            {
                knockBackCounter = knockBackTime;
                knockBackImunity = false;
                this.GetComponent<AIPath>().canMove = isAiActive;
            } 
        }
    }

    private void FixedUpdate()
    {
        // enemy pasisuka pagal vaiksciojimo krypti -M
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        // -M
        if (bullet != null)
        {
            if (!isInAttackRange)
                rangePauseCounter = 0;
            if (rangePauseCounter > 0)
                rangePauseCounter -= Time.deltaTime;
            if (rangePauseCounter <= 0 && isInAttackRange)
                RangeAttack();
        }
    }

    private void Move(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
        anim.SetBool("spotted", true);
    }

    // Enemy attack
    void OnTriggerStay2D(Collider2D other)
    {
        if (attackPauseCounter > 0)
        {
            attackPauseCounter -= Time.deltaTime;
        }

        if (other.CompareTag("Player") && attackPauseCounter <= 0)
        {
            var playerHealth = other.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
                attackPauseCounter = attackPause;

                StartCoroutine(getDamageAnimation());
            }
        }
    }

    IEnumerator getDamageAnimation()
    {
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(3000f);
        anim.SetBool("isHit", false);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        attackPauseCounter = 0;
    }

    void RangeAttack() {
        rangePauseCounter = attackPause;
        GameObject newBullet = Instantiate(bullet, this.transform.position, Quaternion.identity); 
        newBullet.GetComponent<BulletController>().setDirection(target.transform.position - this.transform.position);
        newBullet.GetComponent<BulletController>().setParent(this.gameObject);
        if (bulletSpeed > 0)
            newBullet.GetComponent<BulletController>().speed = bulletSpeed;
        if(bulletDamage > 0)
            newBullet.GetComponent<BulletController>().damage = bulletDamage;
    }
    public void setKnockBackImunity(bool value) { 
        this.knockBackImunity = value;
    }
}
