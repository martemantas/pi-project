using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPassiveObject : MonoBehaviour
{
    public GameObject bullet;

    private void OnCollisionEnder2D(Collider2D col)
    {
        if(col.gameObject == bullet)
        {
            Destroy(gameObject);
            //Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        }
    }
}
