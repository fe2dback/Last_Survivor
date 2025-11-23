using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Target;
    public float dist;
    [Tooltip("조준속도")]
    public float aimSpeed = 3f;


    float mouseX, mouseY;

    void Start()
    {

    }


    void LateUpdate()
    {
        if(!GameManager.Instance.PlayerDead)
        {
            Observer();
        }
        else
        {
            transform.position = Target.position + Target.up * 4f;
            transform.rotation = Quaternion.identity;
            transform.LookAt(Target);
        }
           
    }



    void Observer()
    {
        //Debug.Log(dist);
        float yAngle = Mathf.LerpAngle(transform.eulerAngles.y, Target.eulerAngles.y, 0.5f);
        float xAngle = Mathf.LerpAngle(transform.eulerAngles.x, Target.eulerAngles.x, 0.5f);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        if(PlayerInput.Rifle_FireReady)
        {
            dist -= Time.deltaTime * aimSpeed;
            dist = Mathf.Clamp(dist, 1, 2);
            


        }
        else
        {
            dist += Time.deltaTime * aimSpeed;
            dist = Mathf.Clamp(dist, 1, 2);
        }
        transform.position = Target.position - (rot * Vector3.forward * dist);// + (rot * Vector3.up);
        transform.LookAt(Target);
    }

}
