using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LegacyAnim : MonoBehaviour
{
    Animation anim;
    float speed = 3f;
    void Start()
    {
        anim = GetComponent<Animation>();
    }
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");       
        
        transform.Rotate(0, r * speed, 0);

        if(x!=0 || z!=0) // 키가 입력되면
        {
            anim.CrossFade("RunF", 0.25f);
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);
        }
    }

}