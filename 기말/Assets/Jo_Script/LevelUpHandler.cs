using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpHandler : MonoBehaviour
{
    public static LevelUpHandler Instance;

    public SkillChoiceManager skillChoiceUI; // Ω∫≈≥ »πµÊ
    public PassiveChoiceManager passiveChoiceUI; // Ω∫≈» »πµÊ
    
    private void Awake()
    {
        Instance = this;
    }

    public void OnLevelUp(int level)
    {
        if (level == 3)
            SkillChoiceManager.Instance.OpenChoice(SkillType.Q);

        else if (level == 6)
            SkillChoiceManager.Instance.OpenChoice(SkillType.E);

        else if (level == 9)
            SkillChoiceManager.Instance.OpenChoice(SkillType.R);

        else
        {
            passiveChoiceUI.OpenPassiveUI();
        }

    }
}
