using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyAI : MonoBehaviour
{
    [Header("Place PlayerTransform!!")]
    public Transform player;
    public NavMeshAgent nvAgent; //NavMeshAgent 컴포넌트 참조

    // Animator 컴포넌트 참조
    public Animator anim;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Awake()
    {

        nvAgent = GetComponent<NavMeshAgent>();

        //  Animator 컴포넌트 가져오기
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("MelleEnemyAI: Animator 컴포넌트를 찾을 수 없습니다! 애니메이션이 동작하지 않을 수 있습니다.", this);
        }

        if (nvAgent == null)
        {
            Debug.LogError("MelleEnemyAI: NavMeshAgent 컴포넌트를 찾을 수 없습니다! 적 오브젝트에 추가했는지 확인해 주세요.", this);
        }

        // 플레이어 찾기: 인스펙터에 직접 할당되지 않았을 경우에만
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

    // 적이 죽었을 때 호출될 함수 
    public void Die()
    {
        Debug.Log("적이 죽었습니다! Die 애니메이션 재생");

        // 일단 적의 움직임과 공격을 바로 중단시킨다.
        if (nvAgent != null)
        {
            nvAgent.enabled = false;
            // 적이 죽었을 때 혹시라도 계속 달리기 애니메이션을 끄기 위해
            if (anim != null) anim.SetBool("IsRun", false);
        }

        MelleEnemyAttackController attackController = GetComponent<MelleEnemyAttackController>();
        if (attackController != null)
        {
            attackController.enabled = false;
        }

        // 이 스크립트도 비활성화 (Update 호출 중지)
        this.enabled = false;

        
    }

}