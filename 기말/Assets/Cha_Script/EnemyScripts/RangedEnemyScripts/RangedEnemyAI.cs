using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MelleEnemyAI
{

    [Header("Ranged Move Settings")]
    [Tooltip("이 거리보다 멀면 플레이어를 추격함")]
    public float preferredDistance = 10f;

    [Tooltip("이 거리보다 가까워지면 플레이어에게서 멀어지려고 함")]
    public float minDistance = 5f;

    [Tooltip("이 거리보다 멀어지면 다시 추격을 시작함")]
    public float maxChaseDistance = 25f;

    [Tooltip("플레이어를 바라보는 회전 속도")]
    public float rotationSpeed = 8f;

    protected override void Start()
    {
        base.Start();  

        if (nvAgent == null)
            nvAgent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 부모의 Update 대신, 원거리 전용 이동 로직
        if (player == null || nvAgent == null || !nvAgent.enabled)
        {
            if (anim != null)
            {
                anim.SetBool("IsRun", false);
            }
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 항상 플레이어 방향을 향하도록 회전
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        // 이동/정지 로직
        if (distanceToPlayer > preferredDistance && distanceToPlayer <= maxChaseDistance)
        {
            if (distanceToPlayer > preferredDistance)
            {
                // 너무 멀다 → 무조건 플레이어 쪽으로 접근
                if (nvAgent.isOnNavMesh)
                {
                    nvAgent.isStopped = false;
                    nvAgent.SetDestination(player.position);
                }
            }
            else if (distanceToPlayer < minDistance)
            {
                // 너무 가깝다 → 뒤로 빠지기
                if (nvAgent.isOnNavMesh)
                {
                    nvAgent.isStopped = false;

                    Vector3 retreatDir = (transform.position - player.position).normalized;
                    Vector3 retreatTarget = transform.position + retreatDir * (preferredDistance - distanceToPlayer);

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(retreatTarget, out hit, 3f, NavMesh.AllAreas))
                        nvAgent.SetDestination(hit.position);
                    else
                        nvAgent.SetDestination(transform.position + retreatDir * 2f);
                }
            }
            else
            {
                // 적당한 거리 → 멈춰서 쏘기만
                if (nvAgent.isOnNavMesh)
                {
                    nvAgent.isStopped = true;
                    nvAgent.ResetPath();
                    nvAgent.velocity = Vector3.zero;
                }
            }
        }
        else if (distanceToPlayer < minDistance)
        {
            // 너무 가깝다 → 플레이어에서 멀어지도록 후퇴
            if (nvAgent.isOnNavMesh)
            {
                nvAgent.isStopped = false;

                Vector3 retreatDir = (transform.position - player.position).normalized;
                Vector3 retreatTarget = transform.position + retreatDir * (preferredDistance - distanceToPlayer);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(retreatTarget, out hit, 3f, NavMesh.AllAreas))
                {
                    nvAgent.SetDestination(hit.position);
                }
                else
                {
                    // NavMesh 위에 못 찾으면 걍 반대 방향으로 조금만 이동
                    nvAgent.SetDestination(transform.position + retreatDir * 2f);
                }
            }
        }
        else
        {
            // 적당한 거리 안쪽 → 제자리에서 쏘기만 하면 됨 (공격 컨트롤러가 실제 공격 처리)
            if (nvAgent.isOnNavMesh && !nvAgent.isStopped)
            {
                nvAgent.isStopped = true;
                nvAgent.ResetPath();
                nvAgent.velocity = Vector3.zero;
            }
        }

        // 달리기 애니메이션
        if (anim != null && nvAgent != null)
        {
            bool isMoving = nvAgent.enabled && !nvAgent.isStopped && nvAgent.velocity.magnitude > 0.1f;
            anim.SetBool("IsRun", isMoving);
        }

    }

    // PauseMovement, Die는 부모(MelleEnemyAI)의 것을 그대로 사용
    // EnemyHealth에서 MelleEnemyAI 타입으로 참조하기 때문에 상속 구조를 유지함.
}
