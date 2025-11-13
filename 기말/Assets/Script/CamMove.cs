using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Transform first;
    public Transform Third;
    float dist = 2.0f;
    float height = 0f;
    Transform CamSet;
    bool test =false;

    public static bool FPSviewMode = false;

    float mouseX, mouseY;

    void Start()
    {
        CamSet = GetComponent<Transform>();

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
        Observer();
        if (Input.GetMouseButtonDown(1))
        {
            test = !test;
        }

        if (test == true && PlayerInput.Hand_Rifle == true)
        {
            ObserverAim();
        }



    }

    void firsts()
    {
        float yAngle = Mathf.LerpAngle(CamSet.eulerAngles.y, target.eulerAngles.y, 1);
        float xAngle = Mathf.LerpAngle(CamSet.eulerAngles.x, target.eulerAngles.x, 1);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        CamSet.rotation = rot;
        CamSet.position = first.position;
        
    }

    void ObserverAim()
    {
        first.rotation = target.rotation;
        CamSet.position = Third.position;
        
    }


    void Observer()
    {
        float yAngle = Mathf.LerpAngle(CamSet.eulerAngles.y, target.eulerAngles.y, 0.5f);
        float xAngle = Mathf.LerpAngle(CamSet.eulerAngles.x, target.eulerAngles.x, 0.5f);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        CamSet.position = target.position - (rot * Vector3.forward * dist) + (rot * Vector3.up * height);
        CamSet.LookAt(target);
    }

}
