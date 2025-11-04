using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav : MonoBehaviour
{
    public Transform Player;
    NavMeshAgent nvAgent;
    void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        nvAgent.destination  = Player.position;
    }
}
