using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 3f;
    public GameObject gameObject;
    public string hitSound;
    public string deathSound;

    public void Damage(float damagePoints)
    {
        if (gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (gameObject.GetComponent<PlayerMovement>().isDashing != true)
            {
                health -= damagePoints;
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
            if(this.gameObject.CompareTag("Player"))
                GameObject.FindWithTag("GameOverScreen").GetComponent<GameOverScreen>().Setup();
            FindObjectOfType<AudioManager>().Play(deathSound);
        }
        Debug.Log("Hit: " + this.gameObject.name + " " + health);
    }
}
