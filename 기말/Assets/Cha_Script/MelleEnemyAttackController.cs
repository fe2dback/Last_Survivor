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
    public float attackDamage;//공격 데미지
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

        attackDamage=50.0f*currentLevel; //공격 데미지 계산
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
                // 이미 멈춰있지 않으면 멈추고, 즉시 정지시킨다.
                if (nvAgent.isOnNavMesh && !nvAgent.isStopped)
                {
                    nvAgent.isStopped = true;
                    nvAgent.velocity = Vector3.zero; // 혹시 모를 잔여 움직임 제거
                }
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
            else // 플레이어가 사거리 밖에 있을 때 (추적 상태로 돌아가야 함)
            {
                // !!! 여기는 그냥 두면 돼. MelleEnemyAI가 isStopped가 false면 알아서 추적할 거니까. !!!
                // 만약 isAttacking이 false인데, 공격 때문에 멈춰있었다면 이제 움직여야 해.
                // 이 부분을 MelleEnemyAI가 관리하게 하거나, AttackRoutine 끝에서 명확히 넘겨주는 게 더 좋아.
                // 지금은 MelleEnemyAttackController에서 직접 nvAgent.SetDestination을 호출하지 않고,
                // isStopped = false;로 MelleEnemyAI가 다시 추적하도록 유도하는 방식으로 갈게.
                if (!isAttacking && nvAgent.isOnNavMesh && nvAgent.isStopped)
                {
                    nvAgent.isStopped = false; // 공격이 끝나고 사거리 밖이면 다시 움직이게 해줘야 해.
                                               // MelleEnemyAI가 이것을 보고 SetDestination을 호출할 거야.
                }
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true; //공격 시작 상태로 변경
        lastAttackTime = Time.time; //마지막 공격 시간 갱신

        // 공격 시작 시점에 확실히 멈춤
        if (nvAgent.isOnNavMesh && !nvAgent.isStopped)
        {
            nvAgent.isStopped = true;
            nvAgent.velocity = Vector3.zero;
        }

        Debug.Log("적 공격 준비(애니메이션 안넣음)");

        yield return new WaitForSeconds(attackDelay);

        //공격 판정 시점: 플레이어가 여전히 범위 내에 있는지 다시 확인(어렵게할려면 attackRange 뒤에 수를 줄이셈)
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange + 0.1f)
        {
            //플레이어에게 데미지 주기
            PlayerStates playerStats = player.GetComponent<PlayerStates>(); //PlayerStates 넣어줌
            if (playerStats != null)
            {
                //playerStats.TakeDamage(attackDamage); //PlayerStats의 TakeDamage함수 호출
                playerStats.Hit(attackDamage);
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

        // 공격이 끝나면 MelleEnemyAI가 추적을 시작할 수 있도록 nvAgent.isStopped를 false로 설정해야 해.
        // 이것이 가장 확실하게 제어권을 MelleEnemyAI에게 다시 넘겨주는 방법이야.
        if (nvAgent != null && nvAgent.isOnNavMesh)
        {
            nvAgent.isStopped = false;
            // isStopped가 false가 되면 MelleEnemyAI 스크립트의 Update가 다시 SetDestination을 호출하게 될 거야.
        }

    }
}
