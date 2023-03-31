using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private void SetExperienceBarSize(float experianceNormalized)
    {
        transform.Find("experienceBar").Find("bar").GetComponent<Image>().fillAmount = experianceNormalized;
    }
    private void SetLevelNumber(int levelNumber)
    {
        string newLevel = "LEVEL\n" + levelNumber;
        transform.Find("levelText").GetComponent<TMP_Text>().text = newLevel;
    }
    public void SetLevelSystemAnimated()
    {

        SetLevelNumber(LevelSystem.GetLevelNumber());
        SetExperienceBarSize(LevelSystem.GetExperienceNormalized());
        // Surbscribe to the change events
        LevelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        LevelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        // level changed, update text
        Debug.Log("level changed" + LevelSystem.GetLevelNumber());
        SetLevelNumber(LevelSystem.GetLevelNumber());
    }

    private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        //Experience changed, update bar size
        Debug.Log("experience changed" + LevelSystem.GetExperienceNormalized());
        SetExperienceBarSize(LevelSystem.GetExperienceNormalized());
    }
    private void OnDestroy()
    {
        LevelSystem.OnExperienceChanged -= LevelSystem_OnExperienceChanged;
        LevelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;
    }
}
