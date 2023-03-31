using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static event EventHandler OnExperienceChanged;
   public static event EventHandler OnLevelChanged;
    [SerializeField] private LevelWindow LevelWindow;
    private static int level;
    private static int experience;
    private static int experienceToNextLevel;
    public static int levelPoints;

    public void Awake()
    {
        level = 0;
        experience = 0;
        experienceToNextLevel = 100;
        levelPoints = 0;
        LevelWindow.SetLevelSystemAnimated();

    }
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log("Prideta : " + amount + " , is viso yra : " + experience);
        while (experience >= experienceToNextLevel)
        {
            level++;
            levelPoints++;
            experience -= experienceToNextLevel;
            if (OnLevelChanged != null) { Debug.Log("Level Changed, on system side"); 
                OnLevelChanged(this, EventArgs.Empty); }
        }
        if (OnExperienceChanged != null) { Debug.Log("experience Changed, on system side");
            OnExperienceChanged(this, EventArgs.Empty); }
    }
    public static int GetLevelNumber()
    {
        return level;
    }

    public static float GetExperienceNormalized()
    {
        return (float)experience / experienceToNextLevel;
    }
    public static void SpendLevelPoints(int amount)
    {
        levelPoints -= amount;
    }
    public int GetExperience()
    {
        return experience;
    }
    public int GetExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }
}
