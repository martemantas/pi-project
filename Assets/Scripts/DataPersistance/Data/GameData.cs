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
    public float[] stats;

    public int level;
    public int experience;
    public int experienceToNextLevel;

    public float time;

    public GameData()
    {
        this.money = 0;
        charactersUnlocked = new SerializableDictionary<string, bool>();
        SkillLevels = new int[10];
        stats = new float[10];
        stats = GetStats();
        level = 0;
        experience = 0;
        experienceToNextLevel = 100;
        time = 0;
    }
    public float[] GetStats()
    {
        float[] stats = new float[10];
        stats[0] = 0;
        stats[1] = 1;
        stats[2] = 0.5f;
        stats[3] = 5;
        stats[4] = 3;
        stats[5] = 0.15f;
        stats[6] = 1;
        stats[7] = 0;
        stats[8] = 0.15f;
        stats[9] = 5;
        return stats;
    }
}
