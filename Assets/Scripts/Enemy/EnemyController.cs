using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;        //greitis
    public float checkRadius;  //matymo range
    public float attackRadius; //atakos range

    public float avoidDistance = 2f; // distance at which enemy avoids other enemies
    public float avoidanceForce = 5f; // force of avoidance behavior

    public LayerMask playerMask; //zaidejo layer
    public float scaler = 0.3f;

    public Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 direction;
    private Vector2 avoidVector;

    private bool isInAttackRange;
    private bool isInCheckRange;
    public bool inRoom = false;

    SpriteRenderer spriteRenderer; //enemy sprite -M
    private Animator anim;

    private List<Transform> enemies;

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
    private float speedBackup;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // -M
        target = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();

        enemies = new List<Transform>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            if (enemy.transform != transform)
            {
                enemies.Add(enemy.transform);
            }
        }
        knockBackImunity = false;
        knockBackCounter = knockBackTime;
        speedBackup = speed;
        if (knockBackTime <= 0)
            knockBackTime = 0.5f;
    }

    private void Update()
    {
        if (inRoom)
        {
            isInCheckRange = Physics2D.OverlapCircle(transform.position, checkRadius, playerMask);
            isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, playerMask);

            direction = target.position - transform.position;
            if (knockBackImunity && knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                direction *= -1;
            }
            if (knockBackCounter <= 0 && knockBackImunity)
            {
                knockBackCounter = knockBackTime;
                knockBackImunity = false;
                speed = speedBackup;
            }
            direction.Normalize();
            movement = direction;

            Vector3 avoidance = Vector3.zero;
            foreach (Transform enemy in enemies)
            {
                
                if (enemy != null)
                {
                    // calculate the distance to the other enemy
                    Vector3 enemyDirection = enemy.position - transform.position;
                    float enemyDistance = enemyDirection.magnitude;

                    // check if the other enemy is within the avoidance distance
                    if (enemyDistance <= avoidDistance)
                    {
                        // calculate the avoidance force
                        avoidance += -enemyDirection.normalized * avoidanceForce / enemyDistance;
                    }
                }
            }
            avoidance.Normalize();
            avoidVector = avoidance;
        }
        else
        {
            movement = Vector2.zero;
            avoidVector = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isInCheckRange && !isInAttackRange) // juda link player
        {
            Move(movement*(1-scaler)+avoidVector*scaler);
        }
        else if (isInAttackRange) //jei gali pulti kolkas tik sustoja
        {
            rb.velocity = Vector2.zero;
        }
        else
            rb.velocity = Vector2.zero;

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
                anim.SetTrigger("Attack");
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
    public void backupSpeed() {
        speedBackup = speed;
    }
}
