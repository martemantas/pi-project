using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_health : MonoBehaviour
{
    float health;
    public Slider healthBar;

    public GameObject deathEffect;

    

    public void Update()
    {
        health = GetComponent<HealthController>().health;
        healthBar.value = health;
        if (healthBar.value <= 5)
        {
            GetComponent<Animator>().SetBool("isEngaged", true);
        }
        if (healthBar.value <= 0) 
        {
            Die();
        }
    }

    //useless
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health < 0)
        {
            Die();
        }
        if(health < health / 2)
        {
            GetComponent<Animator>().SetBool("isEngaged", true);
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        GetComponent<Animator>().SetBool("isDead", true);
        Destroy(gameObject);
    }
}
