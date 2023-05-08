using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visit : MonoBehaviour
{
    public bool visited;

    public void VisitRoom()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        visited = true;
		FindObjectOfType<AudioManager>().Play("doorClose");
	}
}
