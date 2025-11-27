using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyAttackController : MonoBehaviour
{
    private EnemyStats enemyStats;
    private int currentLevel; //현재 적 레벨
    public float directDamage = 1;

    [Header("Place PlayerTransform!!")]
    public Transform player;
    public NavMeshAgent nvAgent;

    // MelleEnemyAI에서 Animator를 가져올 참조 변수
    private Animator anim;

    [Header("공격 설정")]
    public float attackRange = 1.8f; //공격 사거리 (기존: 1.5f)
    public float attackCooldown = 2.5f; //공격 쿨타임 (기존: 2f)
    public float attackDamage;//공격 데미지
    public float attackDelay = 0.75f; //공격 판정 전 대기시간
    public float attackHitBuffer = 0.5f; // 공격 판정 시 attackRange에 더할 여유 범위 (기존: 0.2f)

    [Header("공격 후 움직임 설정")]
    public float targetRetreatDistanceFromPlayer = 4.0f; //  공격 후 플레이어로부터 떨어질 목표 거리
    public float retreatSpeedMultiplier = 2.0f; // 후퇴할 때의 속도 배율
    public float reEngageDelay = 0.7f; // 후퇴 후 다시 추격하기 전 잠시 대기 시간 (기존: 0.5f)

    private float lastAttackTime; //마지막 공격 시간을 기록
    private bool isAttacking = false; //현재 공격 동작 중인지 여부
    private bool isRetreating = false; // 현재 후퇴 중인지 여부 (추가)
    private float originalAgentSpeed; // NavMeshAgent의 원래 속도 저장용

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            currentLevel = enemyStats.CurrentEnemyLevel;
        }
        else
        {
            Debug.LogError("EnemyStats 스크립트가 없음");
            currentLevel = 1;
        }

        attackDamage = directDamage * currentLevel;

        // Animator 컴포넌트 가져오기 (MelleEnemyAI와 같은 오브젝트에 있을 경우)
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("MelleEnemyAttackController: Animator 컴포넌트를 찾을 수 없습니다! 애니메이션이 동작하지 않을 수 있습니다.", this);
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (nvAgent == null)
        {
            nvAgent = GetComponent<NavMeshAgent>();
        }

        if (nvAgent == null)
        {
            Debug.Log("NavMeshAgent 컴포넌트가 MelleEnemyAttackController 스크립트에 할당되지 않았거나, 찾을 수 없음!");
            enabled = false;
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
        lastAttackTime = -attackCooldown;

        if (nvAgent != null)
        {
            originalAgentSpeed = nvAgent.speed; // 원래 속도 저장
            nvAgent.stoppingDistance = attackRange - 0.1f; // 공격 사거리 - 0.1f로 설정해서 공격 범위 안에서 멈추도록
            if (nvAgent.stoppingDistance < 0.1f) nvAgent.stoppingDistance = 0.1f; // 최소값 보장
        }
        Debug.Log($"[MelleEnemyAttackController] Enemy stoppingDistance set to: {nvAgent.stoppingDistance}");
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isRetreating)
        {
            return;
        }

        // 플레이어가 공격 사거리 안에 들어왔을 때
        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                if (nvAgent.isOnNavMesh && !nvAgent.isStopped)
                {
                    nvAgent.isStopped = true;
                    nvAgent.velocity = Vector3.zero;
                    Debug.Log("[MelleEnemyAttackController] 적 정지: 공격 범위 진입 및 공격 대기");
                }

                // 플레이어를 바라보게 회전
                Vector3 lookDirection = player.position - transform.position;
                lookDirection.y = 0;
                if (lookDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(lookDirection), Time.deltaTime * nvAgent.angularSpeed);
                }
            }

            // 공격 쿨타임이 지났고, 공격 중이 아니면 AttackRoutine 시작
            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                Debug.Log("[MelleEnemyAttackController] 공격 루틴 시작 조건 충족");
                StartCoroutine(AttackRoutine());
            }
        }
        // 플레이어가 공격 사거리 밖에 있을 때 (추적 상태)
        else
        {
            // 공격 중이 아니며, 현재 멈춰있는 상태라면 다시 움직임을 재개한다.
            // MelleEnemyAI 스크립트가 !nvAgent.isStopped 상태를 확인하고 SetDestination을 호출할 것임.
            if (!isAttacking && nvAgent.isOnNavMesh && nvAgent.isStopped)
            {
                nvAgent.isStopped = false;
                Debug.Log("[MelleEnemyAttackController] 적 재시작: 플레이어가 공격 범위 밖으로 이동");
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // 공격 시작 시점에 확실히 멈춤
        if (nvAgent.isOnNavMesh && !nvAgent.isStopped)
        {
            nvAgent.isStopped = true;
            nvAgent.velocity = Vector3.zero;
        }

        // 공격 애니메이션 재생
        if (anim != null)
        {
            anim.SetTrigger("Attack"); // "Attack" 트리거를 설정해서 공격 애니메이션 재생
            // 혹시 'IsAttack' 같은 bool 값을 쓴다면 SetBool("IsAttack", true);
        }

        Debug.Log("[MelleEnemyAttackController] 적 공격 준비 (애니메이션 넣을 시 대기)");

        yield return new WaitForSeconds(attackDelay);

        // 공격 판정 로직
        float currentDistance = Vector3.Distance(transform.position, player.position);
        if (player != null && currentDistance <= attackRange + attackHitBuffer)
        {
            Debug.Log($"[MelleEnemyAttackController] 플레이어 히트! 현재 거리: {currentDistance:F2} / 판정 범위: {attackRange + attackHitBuffer:F2}");
            PlayerStates playerStats = player.GetComponent<PlayerStates>();
            if (playerStats != null)
            {
                playerStats.Hit(attackDamage);
                Debug.Log("플레이어에게 " + (attackDamage) + "데미지를 입힘!");
            }
            else
            {
                Debug.LogWarning("[MelleEnemyAttackController] 플레이어의 'PlayerStates'스크립트를 찾을 수 없음(데미지 적용 실패)");
            }
        }
        else
        {
            Debug.Log($"[MelleEnemyAttackController] 플레이어가 공격 범위 밖으로 이동하여 공격을 피함! 현재 거리: {currentDistance:F2} / 판정 범위: {attackRange + attackHitBuffer:F2}");
        }

        isAttacking = false; // 공격 종료 시점

        // --- 여기서부터 후퇴(Retreat) 로직 시작 ---
        isRetreating = true;
        anim.SetBool("IsRun", true);

        if (nvAgent != null && nvAgent.isOnNavMesh && nvAgent.enabled)
        {
            isRetreating = true; // 후퇴 시작 상태
            nvAgent.isStopped = false; // 후퇴를 위해 에이전트 활성화 (움직일 수 있도록)
            nvAgent.speed = originalAgentSpeed * retreatSpeedMultiplier; // 후퇴 시 속도 증가


            // 플레이어로부터 'targetRetreatDistanceFromPlayer' 만큼 떨어진 목표 지점 계산
            Vector3 playerPosAtAttack = player.position; // 공격 시점의 플레이어 위치 저장
            Vector3 retreatDirection = (transform.position - playerPosAtAttack).normalized; // 플레이어로부터 멀어지는 방향
            Vector3 targetRetreatPosition = playerPosAtAttack + retreatDirection * targetRetreatDistanceFromPlayer;

            // 후퇴 목표 지점을 NavMesh 상에서 유효한 위치로 보정
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetRetreatPosition, out hit, targetRetreatDistanceFromPlayer + 2f, NavMesh.AllAreas))
            {
                targetRetreatPosition = hit.position;
            }
            else
            {
                targetRetreatPosition = transform.position + retreatDirection * (targetRetreatDistanceFromPlayer + 1f);
                Debug.LogWarning("[MelleEnemyAttackController] 후퇴 목표 지점 NavMesh 찾기 실패, 대체 위치 사용");
            }

            nvAgent.SetDestination(targetRetreatPosition);
            Debug.Log($"[MelleEnemyAttackController] 적 후퇴 시작. 목표 지점 (플레이어로부터): {targetRetreatPosition}");

            float timer = 0f;
            float maxRetreatTime = targetRetreatDistanceFromPlayer / nvAgent.speed + 1f;

            while (nvAgent.pathPending || (nvAgent.isOnNavMesh && nvAgent.remainingDistance > nvAgent.stoppingDistance + 0.1f))
            {
                if (!nvAgent.isOnNavMesh) break;
                if (Vector3.Distance(transform.position, player.position) > targetRetreatDistanceFromPlayer * 2f) break;

                timer += Time.deltaTime;
                if (timer > maxRetreatTime)
                {
                    Debug.LogWarning("[MelleEnemyAttackController] 최대 후퇴 시간 초과, 후퇴 강제 종료");
                    break;
                }
                yield return null;
            }

            nvAgent.speed = originalAgentSpeed;

            if (reEngageDelay > 0)
            {
                if (nvAgent.isOnNavMesh && !nvAgent.isStopped)
                {
                    nvAgent.isStopped = true;
                    nvAgent.velocity = Vector3.zero;
                }
                yield return new WaitForSeconds(reEngageDelay);
            }

            if (nvAgent.isOnNavMesh)
            {
                nvAgent.isStopped = false;
            }
            isRetreating = false;
            Debug.Log("[MelleEnemyAttackController] 적 후퇴 완료 및 추격 재시작 신호 보냄");
        }
        else
        {
            if (nvAgent != null && nvAgent.isOnNavMesh)
            {
                nvAgent.isStopped = false;
            }
            isRetreating = false;
            Debug.Log("[MelleEnemyAttackController] 적 공격 루틴 종료 (후퇴 미적용), 추격 재시작 신호 보냄");
        }
        // --- 후퇴 로직 끝 ---
    }
}