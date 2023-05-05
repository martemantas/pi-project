using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droppedLootBehavior : MonoBehaviour
{
    public float AttractorSpeed;
    public Loot droppedLoot;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, AttractorSpeed * Time.deltaTime);
            if (transform.position == collision.transform.position)
            {
                if (droppedLoot.isHealth)
                {
                    //FindObjectOfType<HealthController>().Heal(droppedLoot.healAmount);
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.GetComponent<HealthController>().Heal(droppedLoot.healAmount);
                    Destroy(this.gameObject);
                }
                if (droppedLoot.isXp)
                {
                    FindObjectOfType<LevelSystem>().AddExperience(droppedLoot.xpAmount);
                    Destroy(this.gameObject);
                }
            }
        }
    }

}
