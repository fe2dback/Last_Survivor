using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Renderer enemyColor;

    private EnemyStats enemyStats;
    private int currentHealth;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        if(enemyStats != null )
        {
            //EnemyStats 15line에서 받아온 값
            currentHealth = enemyStats.MaxHealth; //EnemyStats에서 계산된 MaxHealth로 초기화
        }
        else
        {
            Debug.LogError("EnemyStats 스크립트가 없음");
            currentHealth = 100;
        }
    }

    private void Start()
    {
        enemyColor = GetComponent<Renderer>();
    }

    public void TakeDamate(int damage)
    {
        currentHealth-= damage;
        Debug.Log("적 현재 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("적이 죽었습니다");
        enemyColor.material.color = Color.red;
        Destroy(gameObject, 2f);

        
    }
}
