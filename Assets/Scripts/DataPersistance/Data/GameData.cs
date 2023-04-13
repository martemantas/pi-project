using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public long lastUpdated;

    public int money;

    public SerializableDictionary<string, bool> charactersUnlocked;

    public int[] SkillLevels;

    public int level;
    public int experience;
    public int experienceToNextLevel;

    public GameData()
    {
        this.money = 0;
        charactersUnlocked = new SerializableDictionary<string, bool>();
        SkillLevels = new int[10];
        level = 0;
        experience = 0;
        experienceToNextLevel = 100;
    }
    
}
