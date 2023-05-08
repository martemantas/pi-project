using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour, IDataPersistance
{
    public static SkillTree skillTree;
    private void Awake() => skillTree = this;

    public int[] SkillLevels;
    public int[] tmpSkillLevels;
    public int[] SkillCaps;
    public string[] SkillNames;
    public string[] SkillDescriptions;

    public GameObject ConnectorHolder;
    public List<GameObject> ConnectorList;

    public List<Skill> skillList;
    public GameObject SkillHolder;

    public int SkillPoints;

    private bool updated = false;

    public void LoadData(GameData data)
    {
       tmpSkillLevels = data.SkillLevels;
        skillTree.UpdateAllSkillUi();
    }
    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < SkillLevels.Length; i++)
        {
            if (data.SkillLevels[i] < SkillLevels[i]) 
            {
                data.SkillLevels[i] = SkillLevels[i];

            }
        }

        //Debug.Log("savesdaaaa");
    }

    private void Start()
    {
        SkillPoints = 1;
        SkillLevels = new int[10];
        SkillCaps = new[] { 1, 2, 4, 2, 2, 2, 2, 4, 1, 2 };
        SkillNames = new[] { "Vision", "Damage ", " Damage", "Cooldown", "knockback", "Attack length", "speed", "Dodge", "Dash length", "Health" };
        //skill names
        SkillDescriptions = new[]
        {
            "See next skills",
            "You deal more damage to your enemies!", //skill desc
            "Your special attack does more damage!",
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
        foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>()) skillList.Add(skill);
        //Debug.Log("skillList.Count" + skillList.Count);
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>()) ConnectorList.Add(connector.gameObject);
        //Debug.Log("ConnectorList.Count" + ConnectorList.Count);

        for (int i = 0; i < skillList.Count; i++) skillList[i].id = i;

        skillList[0].ConnectedSkills = new[] { 1, 6, 9 }; // skill conections
        skillList[1].ConnectedSkills = new[] { 2, 3, 4, 5 };
        skillList[6].ConnectedSkills = new[] { 7, 8 };


        if (tmpSkillLevels[0] > 0)
            SkillLevels = tmpSkillLevels;

        skillTree.UpdateAllSkillUi();
    }
    public void Update()
    {
        skillTree.UpdateAllSkillUi();
    }
    public void UpdateAllSkillUi()
    {
        foreach (var skill in skillList) skill.UpdateUI();
    }
    public void SkillBought(int id)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (id == 0) return;
        if (id == 1) { FindAnyObjectByType<PlayerCombat>().damage += 1; ; FindAnyObjectByType<PlayerCombat>().bulletDamage += 1; }
        if (id == 2) FindAnyObjectByType<PlayerCombat>().explosiveDamage += 5;
        if (id == 3) FindAnyObjectByType<PlayerCombat>().specialPause -= 0.5f;
        if (id == 4) FindAnyObjectByType<PlayerCombat>().knockBackStrength += 0.25f;
        if (id == 5) FindAnyObjectByType<PlayerCombat>().attackRange += (float)0.2f;
        if (id == 6) FindAnyObjectByType<PlayerMovement>().movementSpeed += (float)0.5f;
        if (id == 7)
        {
            player.GetComponentInParent<HealthController>().DodgeChance += 10;
        }
        if (id == 8) FindAnyObjectByType<PlayerMovement>().dashLength += (float)0.5f;
        if (id == 9)
        {
            player.GetComponentInParent<HealthController>().unlockedHeal += 1;
            player.GetComponentInParent<HealthController>().Heal(1);
        }
    }

    public void AddSkillPoints()
    {
        SkillPoints++;
        UpdateAllSkillUi();
    }
}
