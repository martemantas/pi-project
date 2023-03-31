using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    public int[] SkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    public GameObject ConnectorHolder;
    public List<GameObject> ConnectorList;

    public List<Skill> skillList;
    public GameObject SkillHolder;

    public int SkillPoints;

    private void Start()
    {
        SkillPoints = 1;
        SkillLevels = new int[8];
        SkillCaps = new[] { 1, 2, 4, 2, 2, 2, 2, 2};
        SkillNames = new[] {"Vision", "Damage ", "Special Attack Damage","Attack length", "Dash length", "speed", "Dodge", "Health"};
        //skill names
        SkillDescriptions = new[]
        {
            "See next skills",
            "You deal more damage to your enemies!", //skill desc
            "Your special attack does more damage!",
            "Attack length increases!",
            "Your dash length increases!",
            "Your movement speed increases",
            "Get a small chance to dodge enemie attacks",
            "Increases amount of hearts",
        };
        // NOTE: 
        //skill names and desc, are counted the way they are placed in object tab
        foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>()) skillList.Add(skill);
        Debug.Log("skillList.Count" + skillList.Count);
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) ConnectorList.Add(connector.gameObject);
        Debug.Log("ConnectorList.Count" + ConnectorList.Count);

        for (int i = 0; i < skillList.Count; i++) skillList[i].id = i;

        skillList[0].ConnectedSkills = new[] { 1, 4, 6, 7}; // skill conections
        skillList[1].ConnectedSkills = new[] { 2 };
        skillList[2].ConnectedSkills = new[] { 3 };
        skillList[4].ConnectedSkills = new[] { 5 };




        skillTree.UpdateAllSkillUi();
    }
    public void UpdateAllSkillUi()
    {
        foreach (var skill in skillList) skill.UpdateUI();
    }
    public void SkillBought(int id)
    {
        if (id == 0) return;
        if (id == 1) { FindAnyObjectByType<PlayerCombat>().damage += 0.2f; ; FindAnyObjectByType<PlayerCombat>().bulletDamage += 0.2f; }
        if (id == 2) FindAnyObjectByType<PlayerCombat>().explosiveDamage += 2;
        if (id == 3) FindAnyObjectByType<PlayerMovement>().dashLength += (float)0.5;
        if (id == 4) FindAnyObjectByType<PlayerCombat>().attackRange += (float)0.2;
        if (id == 5) FindAnyObjectByType<PlayerMovement>().movementSpeed += (float)0.3;
        if (id == 6) FindAnyObjectByType<HealthController>().DodgeChance += 5;
        if (id == 7) FindAnyObjectByType<HealthController>().unlockedHeal += (float)1;

    }
    public void AddSkillPoints()
    {
        SkillPoints++;
        UpdateAllSkillUi();
    }
}
