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

    private MelleEnemyAttackController attackController;

    protected virtual void Start()
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

        attackController = GetComponent<MelleEnemyAttackController>();
    }


    void Update()
    {
        if (player != null && nvAgent != null && nvAgent.enabled && nvAgent.isOnNavMesh)
        {
            //  공격 후 '후퇴 중'이라면, 이동 목적지는 AttackController가 관리하게 두고 여기서는 SetDestination을 덮어쓰지 않는다.
            if (attackController != null && attackController.IsRetreating)
            {
                bool isMovingWhileRetreat = nvAgent.velocity.magnitude > 0.1f;
                if (anim != null)
                {
                    anim.SetBool("IsRun", isMovingWhileRetreat);
                }
                return; // 더 이상 추적 목적지 설정하지 않고 함수 끝
            }

            // 2) 평소(후퇴 중이 아닐 때)는 기존대로 플레이어를 추적
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

    // 적 움직임과 공격을 일시 정지하는 함수
    public void PauseMovement(float duration)
    {
        // 스크립트가 비활성화된 상태(죽은 상태 등)라면 실행하지 않음
        if (!this.enabled) return;

        // 이미 일시 정지 코루틴이 실행 중이라면 멈추고 새로 시작
        StopAllCoroutines(); // 이 스크립트에서 실행 중인 모든 코루틴을 멈춤
        StartCoroutine(PauseMovementCoroutine(duration));
    }

    // 움직임 일시 정지 코루틴
    private IEnumerator PauseMovementCoroutine(float duration)
    {
        bool wasNavAgentEnabled = false;
        bool wasAttackControllerEnabled = false;

        // NavMeshAgent 비활성화 및 애니메이션 끄기
        if (nvAgent != null)
        {
            wasNavAgentEnabled = nvAgent.enabled; // 현재 상태 저장
            nvAgent.isStopped = true;
            nvAgent.enabled = false;
            if (anim != null) anim.SetBool("IsRun", false); // 달리기 애니메이션 끄기
        }
        // 공격 컨트롤러 비활성화
        if (attackController != null)
        {
            wasAttackControllerEnabled = attackController.enabled; // 현재 상태 저장
            attackController.enabled = false;
        }

        yield return new WaitForSeconds(duration); // 정지 시간만큼 대기

        // 정지 시간이 끝난 후 스크립트가 여전히 활성화 상태(죽지 않음)일 경우에만 복구
        if (!this.enabled) yield break;

        // NavMeshAgent 원래 상태로 복구
        if (nvAgent != null && wasNavAgentEnabled)
        {
            nvAgent.enabled = true;
            nvAgent.isStopped = false;
        }
        // 공격 컨트롤러 원래 상태로 복구
        if (attackController != null && wasAttackControllerEnabled)
        {
            attackController.enabled = true;
        }
    }

    // 적이 죽었을 때 호출될 함수 
    public void Die()
    {

        StopAllCoroutines(); // 죽을 때는 실행 중인 모든 코루틴 멈춤 (PauseMovement 포함)

        // NavMeshAgent 비활성화
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
        Destroy(gameObject, 0.5f);

    }

}