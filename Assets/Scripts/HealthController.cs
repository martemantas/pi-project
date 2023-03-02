using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 3f;
    public GameObject gameObject;
    public float damageCooldown = 0.5f;
    private float damageCooldownCounter = 0;

    public void Damage(float damagePoints) {
        if(damageCooldownCounter <= 0)
        {
            if (gameObject.GetComponent<PlayerMovement>() != null)
            {
                if(gameObject.GetComponent<PlayerMovement>().isDashing != true)
                {
                    health -= damagePoints;
                    FindObjectOfType<AudioManager>().Play("PlayerHit");
                    damageCooldownCounter = damageCooldown;
                }
            }
            else
            {
                health -= damagePoints;
                FindObjectOfType<AudioManager>().Play("PlayerHit");
                damageCooldownCounter = damageCooldown;
            }
        }
        //Debug.Log("Hit " + health);
        if (health <= 0) {
        	GameObject.FindWithTag("GameOverScreen").GetComponent<GameOverScreen>().Setup();
            Destroy(this.gameObject);
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
        }
    }

    private void Update()
    {
        if (damageCooldownCounter > 0)
        {
            damageCooldownCounter -= Time.deltaTime;
        }
    } 
}
