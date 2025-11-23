using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int baseHealth = 100; //기본체력
    public int enemyLevel = 1; //기본적 레벨 (적 레벨 자동으로 오르는 코드 짜야함)
    public int baseExp = 15; //기본 경험치 정해놔야 함

    [Header("Check Stats")]
    //SerializeFiled : 인스펙터 창에 변수를 표시할 수 있게함
    [SerializeField] private int maxHealth;

    public int CurrentEnemyLevel => enemyLevel;
    public int MaxHealth => maxHealth; //EnemyHealth에서 maxHealth를 가져갈 수 있게 프로퍼티
    public int ExpReward => enemyLevel * baseExp; //player가 적을 처치할 시 경험치 얻게 코딩해야함

    void Awake() // 게임오브젝트 생성시 가장 먼저 호출되는 함수
    {
        CheckStats();
    }

    
    private void OnValidate() //인스펙터에서 값이 바뀔때마다 재계산
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
