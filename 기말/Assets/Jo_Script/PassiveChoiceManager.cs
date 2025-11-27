using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveChoiceManager : MonoBehaviour
{
    public GameObject choiceUI;
    public Button hpButton;
    public Button damageButton;
    public Button speedButton;

    private void Start()
    {
        hpButton.onClick.AddListener(() => ChoosePassive("HP"));
        damageButton.onClick.AddListener(() => ChoosePassive("DMG"));
        speedButton.onClick.AddListener(() => ChoosePassive("SPD"));
    }

    public void OpenPassiveUI()
    {
        choiceUI.SetActive(true);
        Time.timeScale = 0f; // ∞‘¿” ∏ÿ√„
    }

    void ChoosePassive(string type)
    {
        switch (type)
        {
            case "HP":
                PlayerStatsUpgrade.Instance.IncreaseHP();  
                break;
            case "DMG":
                PlayerStatsUpgrade.Instance.IncreaseDamage();
                break;
            case "SPD":
                PlayerStatsUpgrade.Instance.IncreaseSpeed();
                break;
        }

        choiceUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
