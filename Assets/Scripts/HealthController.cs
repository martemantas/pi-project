using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 3f;
    public GameObject gameObject;

    public void Damage(float damagePoints) {
        health -= damagePoints;
        Debug.Log("Hit " + health);
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }
}
