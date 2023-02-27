using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;        //greitis
    public float checkRadius;  //matymo range
    public float attackRadius; //atakos range

    public LayerMask playerMask; //zaidejo layer

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 direction;

    private bool isInAttackRange;
    private bool isInCheckRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        isInCheckRange = Physics2D.OverlapCircle(transform.position, checkRadius, playerMask);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, playerMask);

        direction = target.position - transform.position;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        if(isInCheckRange && !isInAttackRange) // juda link player
        {
            Move(movement);
        }
        if (isInAttackRange) //jei gali pulti sustoja
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Move(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }
}