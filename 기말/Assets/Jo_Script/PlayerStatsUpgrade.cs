using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUpgrade : MonoBehaviour
{
    public static PlayerStatsUpgrade Instance;

    public float hpBonus = 0f;
    public float damageBonus = 0f;
    public float speedBonus = 0f;

    public PlayerStates player;

    void Awake()
    {
        Instance = this;
    }

    public void IncreaseHP()
    {
        hpBonus = 30f;
        Debug.Log("HP 증가");
        player.ApplyStatChanges();
        hpBonus = 0f;
    }

    public void IncreaseDamage()
    {
        damageBonus = 1f;
        Debug.Log("공격력 증가");
        player.ApplyStatChanges();
        damageBonus = 0f;
    }

    public void IncreaseSpeed()
    {
        speedBonus = 1f;
        Debug.Log("속도 증가");
        player.ApplyStatChanges();
        speedBonus = 0f;
    }
}
