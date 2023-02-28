using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 0.5f;
    public float attackPause;
    private float attackPauseCounter;

    void OnTriggerStay2D(Collider2D other) {
        if (attackPauseCounter > 0)
        {
            attackPauseCounter -= Time.deltaTime;
        }

        if (other.CompareTag("Player") && attackPauseCounter <= 0) {
            var playerHealth = other.GetComponent<HealthController>();
            if (playerHealth != null) {
                playerHealth.Damage(damage);
                attackPauseCounter = attackPause;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        attackPauseCounter = 0;
    }

}
