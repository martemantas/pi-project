using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Boss_Weapon : MonoBehaviour
{
    public int attackDamage = 1;
    public int enragedAttackDamage = 2;

    public Vector3 attackOffset;
    public float attackRange = 2f;
    public LayerMask attackMask;

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            FindObjectOfType<Boss_health>().TakeDamage(attackDamage);
            Debug.Log(FindObjectOfType<Boss_health>());
        }
    }
    public void EnragedAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<Boss_health>().TakeDamage(enragedAttackDamage);
        }
    }
}
