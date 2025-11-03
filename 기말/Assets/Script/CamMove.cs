using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Transform Scope;
    float dist = 4.0f;
    float height = 0f;
    Transform tr;
    public static bool FPSviewMode = false;

    float mouseX, mouseY;

    void Start()
    {
        tr = GetComponent<Transform>();
    }


    public void switchview()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            FPSviewMode = !FPSviewMode;
        }
    }
    void Update()
    {
        switchview();
    }

    void LateUpdate()
    {
        if (FPSviewMode == true)
        {
            
            first();
            if(Input.GetMouseButton(1))
            {
                aim();
            }
           
        }
        else
        {
            observer();
        }
        
    }

    void first()
    {
        float yAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, 1);
        float xAngle = Mathf.LerpAngle(tr.eulerAngles.x, target.eulerAngles.x, 1);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        tr.rotation = rot;
        tr.position = target.position;
        
    }

    void aim()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");
        //tr.rotation = Quaternion.Euler(mouseY, mouseX, 0);


        float yAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, 1);
        float xAngle = Mathf.LerpAngle(tr.eulerAngles.x, target.eulerAngles.x, 1);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        tr.rotation = rot;
        tr.position = Scope.position;
    }


    void observer()
    {
        float yAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, Time.deltaTime * 2);
        float xAngle = Mathf.LerpAngle(tr.eulerAngles.x, target.eulerAngles.x, Time.deltaTime * 2);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);
        tr.LookAt(target);
    }

}
