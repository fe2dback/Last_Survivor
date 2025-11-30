using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyGenerator : MonoBehaviour
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
            Debug.LogError("MelleEnemyGenerator: enemyPrefab이 할당되지 않았습니다!");
            return;
        }

        enemyStatsPrefab = enemyPrefab.GetComponent<EnemyStats>();
        if (enemyStatsPrefab == null)
        {
            Debug.LogError("MelleEnemyGenerator: enemyPrefab에 EnemyStats 컴포넌트가 없습니다!");
        }

        // 플레이어 트랜스폼을 자동으로 찾기 시도 (태그나 이름으로)
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player"); // "Player" 태그를 가진 오브젝트 찾기
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                playerObj = GameObject.Find("Player(Kachujin G Rosales)"); // 특정 이름으로 찾기
                if (playerObj != null)
                {
                    playerTransform = playerObj.transform;
                }
                else
                {
                    Debug.LogWarning("MelleEnemyGenerator: 플레이어 오브젝트를 찾을 수 없습니다. 수동으로 할당해 주세요.");
                }
            }
        }
    }

    void Start()
    {
        // Start에서 바로 스폰할 경우, 플레이어 위치와 상관없이 스폰될 수 있으므로
        // 비활성화 로직을 Start에서도 확인하도록 수정하거나,
        // 필요에 따라 SpawnEnemies() 호출 시점을 다르게 가져가야 합니다.
        // 현재는 예제 목적으로 Start에 그대로 둡니다.
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        // 1. 플레이어가 스폰 비활성화 반경 내에 있는지 확인
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= disableSpawnRadius)
            {
                Debug.Log("플레이어가 스폰 비활성화 반경 내에 있어 적을 생성하지 않습니다.");
                return; // 스폰하지 않고 함수 종료
            }
        }
        else
        {
            Debug.LogWarning("플레이어 트랜스폼이 할당되지 않아 스폰 비활성화 기능이 동작하지 않습니다.");
        }


        if (enemyStatsPrefab == null)
        {
            Debug.LogWarning("스폰 실패: enemyStatsPrefab 참조 없음");
            return;
        }

        int enemyLevel = Mathf.Max(enemyStatsPrefab.enemyLevel, 1);
        int spawnCount = baseSpawnCount * enemyLevel;

        for (int i = 0; i < spawnCount; i++)
        {
            // 1) 대략적인 랜덤 위치
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = transform.position.y;

            // 2) NavMesh 위의 가장 가까운 점 찾기
            NavMeshHit hit;
            float maxDistance = 5f; // 이 반경 안에서 NavMesh를 찾음

            if (NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas))
            {
                // NavMesh 위에서 스폰
                Vector3 spawnPos = hit.position;
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                // 이 위치 근처에 NavMesh가 없으면 스폰 포기 (디버그용 메시지)
                Debug.LogWarning("MelleEnemyGenerator: NavMesh 위를 찾지 못해 이 적 스폰을 건너뜁니다.");
            }
        }
    }

    // Scene 뷰에서 스폰 범위 및 비활성화 범위 시각화
    private void OnDrawGizmosSelected()
    {
        // 스폰 가능 범위 시각화
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // 스폰 비활성화 범위 시각화
        Gizmos.color = disableRadiusColor; // 설정한 비활성화 반경 색상
        Gizmos.DrawWireSphere(transform.position, disableSpawnRadius);
    }
}
