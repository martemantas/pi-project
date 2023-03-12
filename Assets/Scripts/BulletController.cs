using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject target;
    private Vector2 direction;
    private Rigidbody2D rb;
    public float speed;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        direction = target.transform.position - transform.position;
        direction.Normalize();
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            var playerHealth = collision.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
            }
        }
        else if(!collision.CompareTag("Enemy"))
            Destroy(this.gameObject);
    }
}
