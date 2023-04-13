using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeLong : MonoBehaviour
{
    public static SkillTreeLong skillTreeLong;
    private void Awake() => skillTreeLong = this;

    public int[] SkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    public GameObject ConnectorHolder;
    public List<GameObject> ConnectorList;

    public List<SkillLong> skillList;
    public GameObject SkillHolder;

    public int SkillPoints;

    private void Start()
    {
        SkillPoints = 20;
        SkillLevels = new int[10];
        SkillCaps = new[] { 1, 2, 4, 2, 2, 2, 2, 4, 1, 2};
        SkillNames = new[] {"Vision", "Damage ", " Range", "Cooldown", "knockback", "Attack length", "speed", "Dodge", "Dash length", "Health" };
        //skill names
        SkillDescriptions = new[]
        {
            "See next skills",
            "You deal more damage to your enemies!", //skill desc
            "Your knoback range increases!",
            "You can use your special attack earlier",
            "Your special attack will push enemies more",
            "Attack length increases!",
            "Your movement speed increases",
            "Get a small chance to dodge enemie attacks",
            "Your dash length increases!",
            "Increases amount of hearts",

        };
        // NOTE: 
        //skill names and desc, are counted the way they are placed in object tab
        foreach (var skillLong in SkillHolder.GetComponentsInChildren<SkillLong>()) skillList.Add(skillLong);
        //Debug.Log("skillList.Count" + skillList.Count);
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) ConnectorList.Add(connector.gameObject);
       //Debug.Log("ConnectorList.Count" + ConnectorList.Count);

        for (int i = 0; i < skillList.Count; i++) skillList[i].Longid = i;

        skillList[0].ConnectedSkills = new[] { 1, 6, 9}; // skill conections
        skillList[1].ConnectedSkills = new[] { 2, 3, 4, 5};
        skillList[6].ConnectedSkills = new[] { 7, 8 };




        skillTreeLong.UpdateAllSkillUi();
    }
    public void UpdateAllSkillUi()
    {
        foreach (var skill in skillList) skill.UpdateUI();
    }
    public void SkillBought(int id)
    {
        if (id == 0) return;
        if (id == 1) { FindAnyObjectByType<PlayerCombat>().damage += 0.2f; ; FindAnyObjectByType<PlayerCombat>().bulletDamage += 0.2f; }
        if (id == 2) FindAnyObjectByType<PlayerCombat>().knockBackRange += 2;
        if (id == 3) FindAnyObjectByType<PlayerCombat>().specialPause -= 0.25f;
        if (id == 4) FindAnyObjectByType<PlayerCombat>().knockBackStrength += 0.25f;
        if (id == 5) FindAnyObjectByType<PlayerCombat>().attackRange += (float)0.2;
        if (id == 6) FindAnyObjectByType<PlayerMovement>().movementSpeed += (float)0.3;
        if (id == 7) FindAnyObjectByType<HealthController>().DodgeChance += 5;
        if (id == 8) FindAnyObjectByType<PlayerMovement>().dashLength += (float)0.5;
        if (id == 9) FindAnyObjectByType<HealthController>().unlockedHeal += (float)1;


    }
    public void AddSkillPoints()
    {
        SkillPoints++;
        UpdateAllSkillUi();
    }
}
