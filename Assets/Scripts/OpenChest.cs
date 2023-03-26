using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenChest : MonoBehaviour
{
    public GameObject chest_closed, chest_opened;

    void Start()
    {
        chest_closed.SetActive(true);
        chest_opened.SetActive(false);
    }
    void Update()
    {
        /*if(openAllowed && Input.GetKeyDown(KeyCode.E))
        {
            GiveMoney();
        }*/
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("PlayerGhost"))
        {
            /*text.gameObject.SetActive(true);
            openAllowed = true;*/
            chest_closed.SetActive(false);
            chest_opened.SetActive(true);
            GiveMoney();
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        //if (col.CompareTag("Player"))
        if (col.gameObject.name.Equals("PlayerGhost"))
        {
            /*text.gameObject.SetActive(false);
            openAllowed = false;*/
            chest_closed.SetActive(true);
            chest_opened.SetActive(false);
        }
    }
    private void GiveMoney()
    {
        //Debug.Log("Create money bag object"); //to do
    }
}
