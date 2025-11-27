using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceManager : MonoBehaviour
{
    public static SkillChoiceManager Instance;

    public GameObject choiceUI;
    public Button[] choiceButtons;
    public SkillData[] allSkills;

    private SkillType currentType;

    void Awake() { 
        Instance = this;
        Debug.Log("awake");
        }

    public void OpenChoice(SkillType type)
    {
        currentType = type;
        choiceUI.SetActive(true);

        SkillData[] pool = GetSkillsByType(type);
        SkillData[] random3 = GetRandom3(pool);

        for (int i = 0; i < 3; i++)
        {
            int index = i;
            choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = random3[i].skillName;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() =>
            {
                Choose(random3[index]);
            });
        }

        Time.timeScale = 0f;
    }

    SkillData[] GetSkillsByType(SkillType type)
    {
        List<SkillData> list = new List<SkillData>();
        foreach (var s in allSkills)
        {
            if (s.type == type)
                list.Add(s);
        }
        return list.ToArray();
    }

    SkillData[] GetRandom3(SkillData[] pool)
    {
        SkillData[] result = new SkillData[3];
        for (int i = 0; i < 3; i++)
            result[i] = pool[Random.Range(0, pool.Length)];
        return result;
    }

    void Choose(SkillData data)
    {
        Debug.Log("[SkillChoiceManager] Choose 호출: " + (data == null ? "null" : data.skillName));

        if (PlayerSkillManager.Instance == null)
        {
            Debug.LogError("[SkillChoiceManager] PlayerSkillManager.Instance 가 null!");
            return;
        }
        PlayerSkillManager.Instance.AssignSkill(data);
        Debug.Log("[SkillChoiceManager] AssignSkill 호출까지 끝났음");
        choiceUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
