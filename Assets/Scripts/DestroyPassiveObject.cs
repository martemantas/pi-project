using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPassiveObject : MonoBehaviour
{
    public GameObject bullet;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject == bullet)
        {
            Destroy(gameObject);
            //Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        }
    }
}
