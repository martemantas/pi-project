using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SkillTreeLong;

public class SkillLong : MonoBehaviour
{
    public int Longid;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;

    public int[] ConnectedSkills;

    public void UpdateUI()
    {
        TitleText.text = $"{skillTreeLong.SkillLevels[Longid]}/{skillTreeLong.SkillCaps[Longid]}\n{skillTreeLong.SkillNames[Longid]}";
        DescriptionText.text = $"{skillTreeLong.SkillDescriptions[Longid]}\nCost: {skillTreeLong.SkillPoints}/1 LP";

        GetComponent<Image>().color = skillTreeLong.SkillLevels[Longid] >= skillTreeLong.SkillCaps[Longid] ? Color.yellow
            : skillTreeLong.SkillPoints >= 1 ? Color.green : Color.white;

        foreach (var connectedSkill in ConnectedSkills)
        {
            skillTreeLong.skillList[connectedSkill].gameObject.SetActive(skillTreeLong.SkillLevels[Longid] > 0);
            skillTreeLong.ConnectorList[connectedSkill].SetActive(skillTreeLong.SkillLevels[Longid] > 0);
        }
    }
    public void Buy()
    {
        if (skillTreeLong.SkillPoints < 1 || skillTreeLong.SkillLevels[Longid] >= skillTreeLong.SkillCaps[Longid]) return;
        {
            skillTreeLong.SkillPoints -= 1;
            skillTreeLong.SkillLevels[Longid]++;
            skillTreeLong.UpdateAllSkillUi();
            skillTreeLong.SkillBought(Longid);
        }
    }
}
