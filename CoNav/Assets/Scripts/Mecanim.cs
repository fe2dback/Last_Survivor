using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mecanim : MonoBehaviour
{
    Animator anim;
    public Transform player;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > 3.5f)

        {
            anim.SetBool("IsRun", true);
        }        
        else
        {
            anim.SetBool("IsRun", false);
        }
    }
}
