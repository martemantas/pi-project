using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isBossAlive : MonoBehaviour
{
    bool isAlive;
    bool inTheRoom = false;
    float health;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("SceneChanger").gameObject.SetActive(false);
        health = GetComponent<HealthController>().health;
        //transform.Find("healthBar").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (inTheRoom)
        {
            healthBar.gameObject.SetActive(true);
        }*/
        if (health <= 0)
        {
            isAlive = false;
        }
        if (!isAlive)
        {
            transform.Find("SceneChanger").gameObject.SetActive(true);
        }
    }

}
