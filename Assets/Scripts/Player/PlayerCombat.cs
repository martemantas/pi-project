using Pathfinding;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    public bool isLongRange; 

    public float damage = 1.0f;
    private Vector2 attackDirection = Vector2.zero;
    public Transform attackPoint;
    public Transform attackArea;
    public float attackRange = 0.5f;

    public float knockBackRange = 0.5f;
    public float knockBackStrength = 3.0f;

    private float attackPauseCounter;
    public float attackPause;
    private float specialPauseCounter;
    public float specialPause;

    private Vector2 lastDirection = Vector2.zero;
    public GameObject bullet;
    public float bulletSpeed;
    public float bulletDamage;

    public GameObject explosive;
    public float explosionRadius = 0.5f;
    public float explosionTime = 3.0f;
    public float explosiveDamage = 0.5f;

    private Animator animator; //-M

    private void Awake()
    {
        animator = GetComponent<Animator>(); // -M
    }

    void Update()
    {
        //changes the possition of attack point according to the direction of player movement
        if(this.gameObject.GetComponent<PlayerMovement>().getMovementInput() != Vector2.zero)
            attackDirection = this.gameObject.GetComponent<PlayerMovement>().getMovementInput();
        if (attackDirection != Vector2.zero) //TODO: put code in upper if statement if it is not removed
        {
            attackPoint.localPosition = attackDirection * 0.05f;
            //Debug.Log(attackDirection + " " + Mathf.Acos(attackDirection.x) * Mathf.Rad2Deg);
            if (attackArea != null)
            {
                if (lastDirection != attackDirection)
                    //attackArea.rotation = Quaternion.EulerRotation(0,0,Mathf.Acos(attackDirection.x) * Mathf.Rad2Deg);
                    attackArea.RotateAround(this.gameObject.transform.position, Vector3.forward, Mathf.Acos(attackDirection.x) * Mathf.Rad2Deg);
                lastDirection = attackDirection;
            }
           
        }

        if (attackPauseCounter > 0)
            attackPauseCounter -= Time.deltaTime;
        if(specialPauseCounter > 0)
            specialPauseCounter -= Time.deltaTime;

        if (isLongRange)
        {
            if (Input.GetKeyDown(KeyCode.N) && bullet != null)
                LongRangeAttack();
            if (Input.GetKeyDown(KeyCode.M) && specialPauseCounter <= 0)
                SpecialBombAttack();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.N))
                ShortRangeAttack();
            if (Input.GetKeyDown(KeyCode.M) && specialPauseCounter <= 0)
                SpecialKnockAttack();
        }
    }
    void ShortRangeAttack() {
        if (attackPauseCounter <= 0)
        {
            animator.SetTrigger("attack");
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
            attackPauseCounter = attackPause;
        }
    }
    void LongRangeAttack() { 
        if(attackPauseCounter <= 0)
        {
            animator.SetTrigger("shoot");
            GameObject newBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().setDirection(attackDirection*10);
            newBullet.GetComponent<BulletController>().setParent(this.gameObject);
            if (bulletSpeed > 0)
                newBullet.GetComponent<BulletController>().speed = bulletSpeed;
            if (bulletDamage > 0)
                newBullet.GetComponent<BulletController>().damage = bulletDamage;
            attackPauseCounter = attackPause;
            //animator.SetBool("isShooting", true); // -M
            //Debug.Log("shooooooot");

        }
    }
    void SpecialKnockAttack() {
        animator.SetTrigger("special");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(this.gameObject.transform.position, knockBackRange);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy")) 
            {
                EnemyController ec = enemy.GetComponent<EnemyController>();
                ec.setKnockBackImunity(true);
                ec.speed = knockBackStrength;
                ec.GetComponent<AIPath>().canMove = false;
            }
        }
        specialPauseCounter = specialPause;
    }
    void SpecialBombAttack() {
        BulletController newExplosive = Instantiate(explosive, this.transform.position, Quaternion.identity).GetComponent<BulletController>();
        newExplosive.isExplosive = true;
        newExplosive.speed = 0;
        newExplosive.damage = explosiveDamage;
        if(explosionRadius > 0)
            newExplosive.explosionRadius = explosionRadius;
        if(explosionTime > 0)
            newExplosive.explosionTime = explosionTime;
        specialPauseCounter = specialPause;
    }
    private void OnDrawGizmosSelected() //used to draw attack range in scene editor
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(this.gameObject.transform.position, knockBackRange);
    }
}
