using System;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    List<Loot> GetDroppedItems()
    {
        int randomNumber = UnityEngine.Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        return possibleItems;
    }
    public void InstantiateLoot(Vector3 spawnPosition)
    {
        List<Loot> droppedItems = GetDroppedItems();
        if (droppedItems != null)
        {
            foreach (Loot item in droppedItems)
            {
                GameObject lootgameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootgameObject.GetComponent<SpriteRenderer>().sprite = item.lootSprite;

                float dropForce = 1f;
                Vector2 dropDirection = new Vector2(UnityEngine.Random.Range(-0.5f, 1f), UnityEngine.Random.Range(-0.5f, 1f));
                lootgameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
            }
        }
    }
}