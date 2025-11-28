using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    private EnemyStats enemyStats;
    private float currentHealth; //현재 체력
    private MelleEnemyAI melleEnemyAI;

    [Header("피격 시 스턴 설정")]
    public float stunDurationOnHit = 2.0f; // 피격 시 정지 시간 (Inspector에서 조절 가능)

    void Awake()
    {

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
        if (currentHealth <= 0) return; // 이미 죽은 경우 추가 데미지 처리 방지

        currentHealth -= damage;
        Debug.Log("적 현재 체력: " + currentHealth);

        // 데미지를 입었을 때 MelleEnemyAI에 정지 명령
        // 단, 아직 죽지 않았고 스크립트가 활성화된 상태일 때만!
        if (melleEnemyAI != null && this.enabled && currentHealth > 0)
        {
            melleEnemyAI.PauseMovement(stunDurationOnHit);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentHealth <= 0) return; // 이미 죽은 적은 충돌 처리 안함

        Bullet bullet = other.GetComponent<Bullet>();
        Knife knife = other.GetComponent<Knife>();

        if (bullet != null)
        {
            // 총알의 데미지 만큼 체력을 깎음
            TakeDamage(bullet.bulletDamage);
            Debug.Log(currentHealth);
            // 총알은 충돌 후 제거
            Destroy(other.gameObject);
        }
        else if(knife != null)
        {
            TakeDamage(knife.KnifeDamage);
        }
    }

    private void Die()
    {
        Debug.Log("적이 죽었습니다");
        if (melleEnemyAI != null)
        {
            melleEnemyAI.Die(); // MelleEnemyAI가 자신의 컴포넌트 비활성화 및 오브젝트 파괴를 담당
        }
        this.enabled = false; // EnemyHealth 스크립트도 비활성화 (더 이상 데미지 처리 안함)

    }

}
