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

        attackDamage=10*currentLevel; //공격 데미지 계산
    }

    private void Start()
    {
        if (nvAgent == null)
        {
            nvAgent=GetComponent<NavMeshAgent>();
        }

        if (nvAgent == null)
        {
            Debug.Log("NavMeshAgent 컴포넌트가 MelleEnemyAttackController 스크립트에 할당되지 않았거나, 찾을 수 없음!");
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

        float distanceToPlayer=Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)//플레이어가 사거리 안에 들어왔을때
        {
            //공격 중이 아닐때 NavMeshAgent 멈춤
            if (!isAttacking)
            {
                nvAgent.isStopped = true;
            }

            //플레이어를 바라보게 회전
            Vector3 lookDirection = player.position-transform.position;
            lookDirection.y = 0; //Y축 회전 고정(바닥만 보도록)
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(lookDirection),Time.deltaTime*nvAgent.angularSpeed);
            }

            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true; //공격 시작 상태로 변경
        lastAttackTime = Time.time; //마지막 공격 시간 갱신

        Debug.Log("적 공격 준비(애니메이션 안넣음)");

        yield return new WaitForSeconds(attackDelay);

        //공격 판정 시점: 플레이어가 여전히 범위 내에 있는지 다시 확인(어렵게할려면 attackRange 뒤에 수를 줄이셈)
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange + 0.1f)
        {
            //플레이어에게 데미지 주기
            PlayerStates playerStats = player.GetComponent<PlayerStates>();
            if (playerStats != null)
            {
                //playerStats.TakeDamage(attackDamage); //PlayerStats의 TakeDamage함수 호출
                Debug.Log("플레이어에게 " + (attackDamage) + "데미지를 입힘!");
            }
            else
            {
                Debug.Log("플레이어의 'PlayerStats'스크립트를 찾을 수 없음(데미지 적용 실패)");
            }
        }
        else
        {
            Debug.Log("플레이어가 공격 범위 밖으로 이동하여 공격을 피함");
        }
        isAttacking = false;

    }
}
