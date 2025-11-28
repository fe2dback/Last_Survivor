using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyGenerator : MonoBehaviour
{
    [Header("Spawner 설정")]
    public GameObject enemyPrefab;      // 생성할 적 프리팹
    public int baseSpawnCount = 1;      // 기본 적 생성 수 (최소 1 이상)
    public float spawnRadius = 5f;      // 적 출현 범위 반경

    [Header("스폰 비활성화 설정")]
    public Transform playerTransform;   // 플레이어 오브젝트의 Transform
    public float disableSpawnRadius = 20f; // 플레이어가 이 반경 내에 있으면 스폰 비활성화
    public Color disableRadiusColor = Color.red; // 비활성화 반경 기즈모 색상

    private EnemyStats enemyStatsPrefab; // 프리팹에서 EnemyStats 참조

    private void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("RangedEnemyGenerator: enemyPrefab이 할당되지 않았습니다!");
            return;
        }

        enemyStatsPrefab = enemyPrefab.GetComponent<EnemyStats>();
        if (enemyStatsPrefab == null)
        {
            Debug.LogError("RangedEnemyGenerator: enemyPrefab에 EnemyStats 컴포넌트가 없습니다!");
        }

        // 플레이어 트랜스폼 자동 탐색
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                playerObj = GameObject.Find("Player(Kachujin G Rosales)");
                if (playerObj != null)
                {
                    playerTransform = playerObj.transform;
                }
                else
                {
                    Debug.LogWarning("RangedEnemyGenerator: 플레이어 오브젝트를 찾을 수 없습니다. 수동으로 할당해 주세요.");
                }
            }
        }
    }

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        // 1. 스폰 비활성화 체크
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= disableSpawnRadius)
            {
                Debug.Log("플레이어가 스폰 비활성화 반경 내에 있어 원거리 적을 생성하지 않습니다.");
                return;
            }
        }
        else
        {
            Debug.LogWarning("RangedEnemyGenerator: 플레이어 트랜스폼이 없어 비활성화 기능이 동작하지 않습니다.");
        }

        if (enemyStatsPrefab == null)
        {
            Debug.LogWarning("스폰 실패: enemyStatsPrefab 참조 없음");
            return;
        }

        int enemyLevel = Mathf.Max(enemyStatsPrefab.enemyLevel, 1);
        int spawnCount = baseSpawnCount * enemyLevel;

        // 2. 원거리 적 스폰
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = transform.position.y;

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = disableRadiusColor;
        Gizmos.DrawWireSphere(transform.position, disableSpawnRadius);
    }
}

