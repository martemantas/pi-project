using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float damage = 1.0f;
    private float attackPauseCounter;
    public float attackPause;

    void Update()
    {
        //changes the possition of attack point according to the direction of player movement
        Vector2 attackDirection = this.gameObject.GetComponent<PlayerMovement>().getMovementInput();
        if(attackDirection != Vector2.zero)
            attackPoint.localPosition = attackDirection * 0.05f;

        if (attackPauseCounter > 0)
        {
            attackPauseCounter -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            Attack();
            attackPauseCounter = attackPause;
        }
    }
    void Attack() {
        if (attackPauseCounter <= 0)
        {
            //gets all colliders which were in attack point circle
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
            string lastName = "";
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.gameObject.CompareTag("Enemy") && enemy.name != lastName) //due to enemies having two colliders checks if last hit enemy is not the same
                {
                    enemy.GetComponent<HealthController>().Damage(damage);
                    lastName = enemy.name;
                }
            }
        }
    }
    private void OnDrawGizmosSelected() //used to draw attack range in scene editor
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
