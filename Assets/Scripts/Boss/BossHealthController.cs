using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    public float health = 20f;
    public float unlockedHeal = 3f;
    public GameObject gameObject;
    public string hitSound;
    public string deathSound;
    public string sound;
    public int DodgeChance;
    public ParticleSystem deathEffect;
    private float MaxHealth;

    // -M
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        unlockedHeal = health;
        MaxHealth = health;
    }
    private void Update()
    {
        if(health <= MaxHealth / 2)
        {
            anim.SetBool("Evolved", true);
        }
    }
    //  
    public void Heal(int healAmount)
    {
        if (unlockedHeal < healAmount + health)
        {
            health = unlockedHeal;
        }
        else
        {
          health += (float)healAmount;
        }
    }
    public void Damage(float damagePoints)
    {
        int digit = Random.Range(0, 100);
        if (gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (gameObject.GetComponent<PlayerMovement>().isDashing != true)
            {
                if (digit <= DodgeChance)
                {
                    Debug.Log("Dodged this");
                    return;
                }
                health -= damagePoints;
                anim.Play("Hit"); // -M
                FindObjectOfType<AudioManager>().Play(hitSound);
            }
        }
        else
        {
            health -= damagePoints;
            FindObjectOfType<AudioManager>().Play(hitSound);
        }
        if (health <= 0)
        {
            
            Destroy(this.gameObject);

            if (this.gameObject.CompareTag("Player"))
            {
                GameObject.FindWithTag("GameOverScreen").GetComponent<GameOverScreen>().Setup();
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            else
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
                GetComponent<LootBag>().InstantiateLoot(transform.position);
            }

            RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine()); ; //
            if (digit >= 75)
            {
                FindObjectOfType<AudioManager>().Play(sound);
            }
            else
            {
                FindObjectOfType<AudioManager>().Play(deathSound);
            }

        }
        Debug.Log("Hit: " + this.gameObject.name + " " + health);
    }
}
