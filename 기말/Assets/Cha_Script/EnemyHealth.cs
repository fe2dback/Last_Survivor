using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Renderer enemyColor;
    
    private EnemyStats enemyStats;
    private float currentHealth; //현재 체력
    private MelleEnemyAI melleEnemyAI;

    void Awake()
    {

        enemyColor = GetComponent<Renderer>();
        enemyStats = GetComponent<EnemyStats>();
        melleEnemyAI = GetComponent<MelleEnemyAI>();

        if (melleEnemyAI == null)
        {
            Debug.LogError("EnemyHealth: MelleEnemyAI 컴포넌트를 찾을 수 없습니다! 같은 오브젝트에 붙어있나요?", this);
        }

        //originalEnemyColor = enemyColor.material.color;
        if (enemyStats != null )
        {
            //EnemyStats 16line에서 받아온 값
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
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth-= damage;
        Debug.Log("적 현재 체력: " + currentHealth);

        if (melleEnemyAI != null)
        {
            melleEnemyAI.ApplyHitEffect(0.2f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Knife knife = other.GetComponentInParent<Knife>();

        if (bullet != null)
        {
            // 총알의 데미지 만큼 체력을 깎음
            TakeDamage(bullet.bulletDamage);

            // 총알은 충돌 후 제거
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "knife")
        {
            TakeDamage(knife.KnifeDamage);
            
        }
    }

    private void Die()
    {
        Debug.Log("적이 죽었습니다");
        enemyColor.material.color = Color.gray;
        Destroy(gameObject, 0.5f);

    }

}
