using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject parent;
    private Vector2 direction;
    private Rigidbody2D rb;
    public float speed;
    public float damage;
    public bool isExplosive = false;
    public float explosionRadius = 0;
    public float explosionTime;
    private float explosionTimeCounter;
    public GameObject explosion;

    public void setDirection(Vector2 dir) { 
        this.direction = dir;
    }
    public void setParent(GameObject obj) { 
        parent = obj;
    }
    // Start is called before the first frame update
    void Start()
    {
        direction.Normalize();
        rb = this.GetComponent<Rigidbody2D>();
        explosionTimeCounter = explosionTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isExplosive)
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        else
        {
            if (explosionTimeCounter > 0)
                explosionTimeCounter -= Time.deltaTime;
            else
                Explode();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isExplosive)
        {
            if (parent.CompareTag("Enemy"))
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
                else if (!collision.CompareTag("Enemy"))
                    Destroy(this.gameObject);
            }
            else if (parent.CompareTag("Player"))
            {
                if (collision.CompareTag("Enemy"))
                {
                    Debug.Log("Bullet died");
                    Destroy(this.gameObject);
                    var enemyHealth = collision.GetComponent<HealthController>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.Damage(damage);
                    }
                }
                else if (!collision.CompareTag("Enemy") && !collision.CompareTag("Player") && !collision.CompareTag("Room"))
                {
                    Destroy(this.gameObject);
                    Debug.Log("Bullet died");
                }
            }
        }
    }
    private void Explode() {
        //GameObject explosion = (GameObject)Resources.Load("Explosion", typeof(GameObject));
        //explosion.GetComponentInChildren<ParticleSystem>().GetComponent<ParticleSystem>().Play();
        Collider2D[] enemies = Physics2D.OverlapCircleAll(this.gameObject.transform.position, explosionRadius);
        string lastName = "";
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy") && enemy.name != lastName) //due to enemies having two colliders checks if last hit enemy is not the same
            {
                enemy.GetComponent<HealthController>().Damage(damage);
                lastName = enemy.name;
            }
        }
        Destroy(this.gameObject);
    }
}