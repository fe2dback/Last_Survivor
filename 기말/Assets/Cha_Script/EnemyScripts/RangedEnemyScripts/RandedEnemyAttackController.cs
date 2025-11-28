using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandedEnemyAttackController : MonoBehaviour
{
    private EnemyStats enemyStats;
    private int currentLevel;
    public float directDamage = 5f;   // 레벨 1 기준 기본 데미지

    [Header("Target / Components")]
    public Transform player;
    public NavMeshAgent nvAgent;
    private Animator anim;

    [Header("Ranged Attack Settings")]
    [Tooltip("이 거리 이내일 때 공격 시도")]
    public float attackRange = 15f;

    [Tooltip("공격 쿨타임(초)")]
    public float attackCooldown = 1.5f;

    [Tooltip("발사 모션 후 실제 총알이 나가는 딜레이(초)")]
    public float attackDelay = 0.15f;

    [Tooltip("공격 시 데미지 (EnemyStats 레벨에 따라 계산된 값)")]
    public float attackDamage;

    [Header("Projectile Settings")]
    [Tooltip("적 총알 프리팹 (Rigidbody + 충돌 데미지 스크립트 필요)")]
    public GameObject bulletPrefab;

    [Tooltip("총알이 발사될 총구 위치")]
    public Transform firePoint;

    [Tooltip("총알 속도")]
    public float bulletSpeed = 25f;

    [Tooltip("총알이 자동으로 파괴되기까지의 시간")]
    public float bulletLifeTime = 5f;

    private float lastAttackTime;
    private bool isAttacking = false;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            currentLevel = enemyStats.CurrentEnemyLevel;
        }
        else
        {
            Debug.LogError("RandedEnemyAttackController: EnemyStats 컴포넌트를 찾을 수 없습니다.", this);
            currentLevel = 1;
        }

        attackDamage = directDamage * currentLevel;

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("RandedEnemyAttackController: Animator 컴포넌트를 찾을 수 없습니다!", this);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                
                playerObj = GameObject.Find("Player(Kachujin G Rosales)");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }
        }

        if (nvAgent == null)
        {
            nvAgent = GetComponent<NavMeshAgent>();
        }

        lastAttackTime = -attackCooldown;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 사거리 밖이면 공격 안 함
        if (distanceToPlayer > attackRange) return;

        // 쿨타임 체크
        if (Time.time < lastAttackTime + attackCooldown) return;

        // 이미 공격 중이면 또 공격 안 함
        if (isAttacking) return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // 공격 시 잠깐 정지(필요 없다면 제거해도 됨)
        if (nvAgent != null && nvAgent.isOnNavMesh)
        {
            nvAgent.velocity = Vector3.zero;
        }

        // 플레이어를 바라보게 회전
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = targetRot;
        }

        // 공격 애니메이션
        if (anim != null)
        {
            anim.SetTrigger("Attack");   // 기존 근접처럼 "Attack" 트리거 사용
        }

        // 애니메이션 상 발사 타이밍까지 대기
        yield return new WaitForSeconds(attackDelay);

        // 실제 총알 발사
        FireBullet();

        isAttacking = false;
    }

    private void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("RandedEnemyAttackController: bulletPrefab 또는 firePoint가 설정되지 않았습니다.", this);
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Rigidbody로 앞으로 날려보내기
        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }

        // Bullet 스크립트가 있다면 데미지 설정
        Bullet bulletComp = bulletObj.GetComponent<Bullet>();
        if (bulletComp != null)
        {
            bulletComp.bulletDamage = attackDamage;
        }

        // 필요하다면 EnemyBullet 같은 별도 스크립트를 써도 됨

        Destroy(bulletObj, bulletLifeTime);
    }
}
