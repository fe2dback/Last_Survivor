using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CCMove : MonoBehaviour
{
    CharacterController CC;
    float z, x;
    float gravity = 0.3f;
    float yAxis = 0f;
    int speed = 5;
    Vector3 sumVector, xVector, zVector, yVector;

    void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    void Update()
    {
        z = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");

        zVector = transform.forward * speed * Time.deltaTime * z;
        xVector = transform.right * speed * Time.deltaTime * x;

        if (CC.isGrounded == true)
        {
            yAxis = 0f;
            if (Input.GetButton("Jump"))
                yAxis = 0.08f;
        }
        else
        {
            yAxis -= gravity * Time.deltaTime;
        }
        yVector = new Vector3(0, yAxis, 0);

        sumVector = xVector + zVector + yVector;
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        CC.Move(sumVector);
    }
}

