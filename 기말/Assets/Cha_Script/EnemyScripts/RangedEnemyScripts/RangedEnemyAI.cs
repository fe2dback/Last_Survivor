using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MelleEnemyAI
{
    [Header("Ranged Move Settings")]
    [Tooltip("이 거리보다 멀면 플레이어를 추격함")]
    public float preferredDistance = 15f;

    [Tooltip("이 거리보다 가까워지면 플레이어에게서 멀어지려고 함")]
    public float minDistance = 7f;

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
        if (player == null || nvAgent == null || !nvAgent.enabled)
        {
            if (anim != null)
                anim.SetBool("IsRun", false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어를 바라보도록 회전
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        bool shouldMove = false;

        // ===== 이동 로직 =====
        if (distanceToPlayer > preferredDistance)
        {
            // 너무 멀다 → 플레이어를 향해 추격
            if (nvAgent.isOnNavMesh)
            {
                nvAgent.isStopped = false;
                nvAgent.SetDestination(player.position);
                shouldMove = true;
            }
        }
        else if (distanceToPlayer < minDistance)
        {
            // 너무 가깝다 → 플레이어에게서 멀어지기
            if (nvAgent.isOnNavMesh)
            {
                nvAgent.isStopped = false;
                shouldMove = true;

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
            // 적당한 거리 → 멈춤 (여기서 공격만 수행)
            if (nvAgent.isOnNavMesh)
            {
                nvAgent.isStopped = true;
                nvAgent.ResetPath();
                nvAgent.velocity = Vector3.zero;
            }
            shouldMove = false;
        }

        // ===== 달리기 애니메이션 =====
        if (anim != null)
            anim.SetBool("IsRun", shouldMove);
    }
}
