using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyAttackController : MonoBehaviour
{
    private EnemyStats enemyStats;
    private int currentLevel; //현재 적 레벨

    [Header("Place PlayerTransform!!")]
    public Transform player;
    public NavMeshAgent nvAgent;

    [Header("공격 설정")]
    public float attackRange = 1.5f; //공격 사거리
    public float attackCooldown = 2f; //공격 쿨타임
    public int attackDamage;//공격 데미지
    public float attackDelay = 0.5f; //공격 판정 

    private float lastAttackTime; //마지막 공격 시간을 기록
    private bool isAttacking = false; //현재 공격 동작 중인지 여부

    void Awake()
    {
        enemyStats =GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            //EnemyStats 16line에서 받아온 값
            currentLevel = enemyStats.CurrentEnemyLevel; //EnemyStats에서 계산된 MaxHealth로 초기화
        }
        else
        {
            Debug.LogError("EnemyStats 스크립트가 없음");
            currentLevel = 1;
        }
    }

    private void Start()
    {
        if (nvAgent == null)
        {
            nvAgent=GetComponent<NavMeshAgent>();
        }

        if (nvAgent == null)
        {
            Debug.Log("NavMeshAgent 컴포넌트가 MelleEnemyAttackController 스크립트에 할당되지 않았거나, 찾을 수 없다!");
            enabled = false; //스크립트 비활성화
            return;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player(Kachujin G Rosales)");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        lastAttackTime = -attackCooldown; // 시작하고서 바로 공격할 수 있도록 초기화
    }

    void Update()
    {
        if (player == null) return; //플레이어가 죽으면 아무것도 안함, //이 객체가 destroy가 안된다는 뜻
    }
}
