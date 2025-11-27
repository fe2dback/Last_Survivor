using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyAI : MonoBehaviour
{
    [Header("Place PlayerTransform!!")]
    public Transform player;
    public NavMeshAgent nvAgent; //NavMeshAgent 컴포넌트 참조

    // ★ 추가 ★ Animator 컴포넌트 참조
    public Animator anim;

    private void Awake()
    {

        nvAgent = GetComponent<NavMeshAgent>();

        // ★ 추가 ★ Animator 컴포넌트 가져오기
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("MelleEnemyAI: Animator 컴포넌트를 찾을 수 없습니다! 애니메이션이 동작하지 않을 수 있습니다.", this);
        }

        if (nvAgent == null)
        {
            Debug.LogError("MelleEnemyAI: NavMeshAgent 컴포넌트를 찾을 수 없습니다! 적 오브젝트에 추가했는지 확인해 주세요.", this);
        }

        // 플레이어 찾기: 인스펙터에 직접 할당되지 않았을 경우에만 찾자.
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
        if (player != null && nvAgent != null && nvAgent.enabled)
        {
            bool isMoving = nvAgent.velocity.magnitude > 0.1f;

            if (anim != null)
            {
                anim.SetBool("IsRun", isMoving);
            }

            if (!nvAgent.isStopped)
            {
                nvAgent.SetDestination(player.position);
            }
        }
        else if (anim != null)
        {
            anim.SetBool("IsRun", false);
        }
    }

    // ★ 추가 ★ 적이 죽었을 때 호출될 함수 (이 부분은 유지)
    public void Die()
    {
        Debug.Log("적이 죽었습니다! Die 애니메이션 재생");

        // 애니메이션 재생
        if (anim != null)
        {
            anim.SetTrigger("Die"); // "Die" 트리거를 설정해서 죽는 애니메이션 재생
            // 애니메이션 클립의 길이를 확인하고 그 시간만큼 기다렸다가 오브젝트 비활성화 등을 처리하면 좋아.
            // 예를 들어: StartCoroutine(DieSequence());
        }

        // NavMeshAgent 비활성화 (더 이상 움직이지 않도록)
        if (nvAgent != null)
        {
            nvAgent.enabled = false;
        }

        // 공격 컨트롤러도 비활성화
        MelleEnemyAttackController attackController = GetComponent<MelleEnemyAttackController>();
        if (attackController != null)
        {
            attackController.enabled = false;
        }

        // 이 스크립트도 비활성화 (더 이상 Update 호출되지 않도록)
        this.enabled = false;

        // 추가적으로 Collider 비활성화, 오브젝트 풀에 반환, Destroy(gameObject, 애니메이션 시간) 등
    }
}