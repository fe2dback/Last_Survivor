using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMove2 : MonoBehaviour
{
    Rigidbody player;
    Animator animator;
    Vector3 pos;
    float speed = 3f;
    float gravity = 0.3f;

    void Start()
    {
        player = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(z, 0, x).normalized * speed;
        Vector3 newVelocity = new Vector3(move.x, player.velocity.y, move.z);

        player.velocity = newVelocity;

    }


    private void Move()
    {
           
    }
}
