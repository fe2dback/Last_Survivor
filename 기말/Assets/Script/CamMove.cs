using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Transform aim;
    public float dist = 5f;
    public float height = 0f;
    bool test =false;

    public static bool FPSviewMode = false;

    float mouseX, mouseY;

    void Start()
    {

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




    }



    void Observer()
    {
        float yAngle = Mathf.LerpAngle(transform.eulerAngles.y, target.eulerAngles.y, 0.5f);
        float xAngle = Mathf.LerpAngle(transform.eulerAngles.x, target.eulerAngles.x, 0.5f);
        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = target.position - (rot * Vector3.forward * dist) + (rot * Vector3.up * height);
        transform.LookAt(aim);
    }

}
