using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class OffMeshLinkMove : MonoBehaviour
{    
    OffMeshLinkData data;
    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    IEnumerator Start()
    {
        while (true)
        {            
            yield return new WaitUntil(() => IsOnOffMeshLink());
            //linkType이 자동이면(대리자가 false면 대기)통과 후 포물선 운동   
            yield return StartCoroutine(parabola());                 
        }
    }

    public bool IsOnOffMeshLink()
    {
        if (agent.isOnOffMeshLink) // NavMeshAgent가 OffMeshLink에 도착하면
        {            
            data = agent.currentOffMeshLinkData;
            if (data.linkType == OffMeshLinkType.LinkTypeJumpAcross ||
                data.linkType == OffMeshLinkType.LinkTypeDropDown)
            {
                agent.speed = 1.7f; // 속도 줄이기(자동)  
                return true;
            }else if (data.linkType == OffMeshLinkType.LinkTypeManual)
            {
                agent.speed = 0.7f; // 속도 줄이기(수동)                
            }
        }
        else
        {
            agent.speed = 3.5f; // 속도 회복  
        }
        return false;
    }
   
    IEnumerator parabola()
    {
        float v0 = 7f; // 포물선 운동 초기 속도
        float t = 0;
        while (t < 0.6f) //0.6초 동안
        {
            t += Time.deltaTime;
            Vector3 pos = Vector3.Lerp(transform.position, data.endPos, t); //x,z 좌표 구하기

            //높이(y)값 수정후 적용
            pos.y = transform.position.y + (v0 * t * Mathf.Sin(50 * Mathf.Deg2Rad)) - (9.8f * t * t) / 2;
            transform.position = pos;

            yield return null;
        }
    }
}