using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public GameObject dropObject;
    public string lootName;
    public int dropChance;
    public bool isHealth;
    public int healAmount;
    public bool isXp;
    public int xpAmount;


    public Loot(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }

}
