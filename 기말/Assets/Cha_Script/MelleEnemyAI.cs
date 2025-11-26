using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyAI : MonoBehaviour
{
    [Header("Place PlayerTransform!!")]
    public Transform player;
    public NavMeshAgent nvAgent; //NavMeshAgent 컴포넌트 참조


    Renderer enemyColor;
    private Renderer enemyRenderer;
    private Color originalEnemyColor;

    private void Awake()
    {
        enemyRenderer=GetComponent<Renderer>();
        enemyColor = GetComponent<Renderer>();
        nvAgent = GetComponent<NavMeshAgent>();

        if (enemyRenderer != null)
        {
            originalEnemyColor = enemyRenderer.material.color; // 게임 시작할 때 원래 색상 저장
        }
        else
        {
            Debug.LogWarning("MelleEnemyAI: Renderer 컴포넌트를 찾을 수 없습니다! 피격 시 색상 변경이 동작하지 않을 수 있습니다.", this);
        }

        if (nvAgent == null)
        {
            Debug.LogError("MelleEnemyAI: NavMeshAgent 컴포넌트를 찾을 수 없습니다! 적 오브젝트에 추가했는지 확인해 주세요.", this);
        }

        // 플레이어 찾기: 인스펙터에 직접 할당되지 않았을 경우에만 찾자.
        // 매 프레임 GameObject.Find를 호출하는 건 성능에 안 좋고, Awake나 Start에서 한 번만 하는 게 좋아.
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player(Kachujin G Rosales)");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("MelleEnemyAI: 'Player(Kachujin G Rosales)' 오브젝트를 찾을 수 없습니다. 인스펙터에 플레이어를 수동으로 할당했거나, 태그를 사용해 찾는 걸 고려해보세요.", this);
            }
        }
    }


    void Update()
    {
        // 플레이어가 있고, NavMeshAgent가 활성화되어 있고, 현재 멈춰있지 않을 때만 따라감
        if (player != null && nvAgent != null && nvAgent.enabled && !nvAgent.isStopped)
        {
            nvAgent.SetDestination(player.position);
        }
    }

    public void ApplyHitEffect(float stopDuration = 0.1f) // 외부에서 호출될 공용 함수
    {
        // 기존에 진행 중이던 피격 효과 코루틴이 있다면 멈추고 새로 시작
        // 이렇게 해야 연속으로 맞았을 때 색상이나 움직임이 꼬이지 않음
        StopCoroutine("HandleHitEffectCoroutine"); // 코루틴 이름을 문자열로 넘기면 멈출 수 있음
        StartCoroutine(HandleHitEffectCoroutine(stopDuration));
    }

    //실제 피격 처리 효과 담당 코루틴
    private IEnumerator HandleHitEffectCoroutine(float duration)
    {
        // 1. 시각 효과: 빨간색으로 변경
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.red;
        }

        // 2. NavMeshAgent 정지: 따라오는 걸 멈춤
        if (nvAgent != null && nvAgent.enabled)
        {
            nvAgent.isStopped = true; // 여기에서 움직임을 멈춤
            nvAgent.velocity = Vector3.zero; // 혹시 모를 잔여 움직임을 없애고 싶다면 추가
        }

        // 지정된 시간만큼 정지. (여기서 적이 멈춰있는 시간을 조절 가능.)
        yield return new WaitForSeconds(duration);

        // 3. 시각 효과: 원래 색상으로 복귀
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalEnemyColor;
        }

        // 4. NavMeshAgent 재시작: 다시 플레이어를 따라감
        if (nvAgent != null && nvAgent.enabled)
        {
            nvAgent.isStopped = false; // 여기에서 다시 움직임
            // 멈춰있는 동안 플레이어 위치가 바뀌었을 수 있으니 목표를 다시 설정해 주는 게 좋음
            if (player != null)
            {
                nvAgent.SetDestination(player.position);
            }
        }
    }
}
