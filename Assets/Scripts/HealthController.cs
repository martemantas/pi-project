using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 3f;
    public GameObject gameObject;

    public void Damage(float damagePoints)
    {
        if (gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (gameObject.GetComponent<PlayerMovement>().isDashing != true)
            {
                health -= damagePoints;
                FindObjectOfType<AudioManager>().Play("PlayerHit");
            }
        }
        else
        {
            health -= damagePoints;
            FindObjectOfType<AudioManager>().Play("PlayerHit");
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameObject.FindWithTag("GameOverScreen").GetComponent<GameOverScreen>().Setup();
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
        }
    }
}
