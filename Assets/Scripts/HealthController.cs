using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 3f;
    public GameObject gameObject;
    public string hitSound;
    public string deathSound;
    public string sound;

    // -M
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    //

    public void Damage(float damagePoints)
    {
        int digit = Random.Range(0, 100);
        if (gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (gameObject.GetComponent<PlayerMovement>().isDashing != true)
            {
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
                GameObject.FindWithTag("GameOverScreen").GetComponent<GameOverScreen>().Setup();
            else
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
