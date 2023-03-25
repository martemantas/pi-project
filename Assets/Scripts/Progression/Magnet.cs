using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float AttractorSpeed;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, AttractorSpeed * Time.deltaTime);
        }
    }
}
