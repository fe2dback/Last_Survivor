using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int baseHealth = 100; //기본체력
    public int enemyLevel = 1; //기본적 레벨 (적 레벨 자동으로 오르는 코드 짜야함)

    [Header("Check Stats")]
    //SerializeFiled : 인스펙터 창에 변수를 표시할 수 있게함
    [SerializeField] private int maxHealth;

    public int MaxHealth => maxHealth; //다른 스크립트에서 maxHealth를 가져갈 수 있게 프로퍼티
    
    void Awake()
    {
        CheckStats();
    }

    //인스펙터에서 값이 바뀔때마다 재계산
    private void OnValidate()
    {
        CheckStats();
    }

    private void CheckStats()
    {
        maxHealth = baseHealth * enemyLevel;
        if (maxHealth <= 0)
        {
            maxHealth = baseHealth;
        }
    }

    
}
