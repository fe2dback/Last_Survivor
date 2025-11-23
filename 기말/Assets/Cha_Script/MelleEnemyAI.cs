using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelleEnemyAI : MonoBehaviour
{
    [Header("Place PlayerTransform!!")]
    public Transform player;
    public NavMeshAgent nvAgent; //NavMeshAgent 컴포넌트 참조
    
    void Start()
    {
        nvAgent=GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player(Kachujin G Rosales)");
            if (playerObj != null)
            {
                player=playerObj.transform;
            }
        }
    }

    
    void Update()
    {
        if (player != null)
        {
            nvAgent.SetDestination(player.position);
        }
    }
}
