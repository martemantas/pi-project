using System.Collections;
using System.Collections.Generic;
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

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 direction;
    private Vector2 avoidVector;

    private bool isInAttackRange;
    private bool isInCheckRange;
    public bool inRoom = false;

    SpriteRenderer spriteRenderer; //enemy sprite -M

    private List<Transform> enemies;

    // Enemy attack
    public float damage = 0.5f;
    public float attackPause;
    private float attackPauseCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // -M

        enemies = new List<Transform>();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            if (enemy.transform != transform)
            {
                enemies.Add(enemy.transform);
            }
        }
    }

    private void Update()
    {
        if (inRoom)
        {


            isInCheckRange = Physics2D.OverlapCircle(transform.position, checkRadius, playerMask);
            isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, playerMask);

            direction = target.position - transform.position;
            direction.Normalize();
            movement = direction;

            Vector3 avoidance = Vector3.zero;
            foreach (Transform enemy in enemies)
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
        if(isInCheckRange && !isInAttackRange) // juda link player
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
    }

    private void Move(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
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
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        attackPauseCounter = 0;
    }
}
