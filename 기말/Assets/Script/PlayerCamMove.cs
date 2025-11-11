using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamMove : MonoBehaviour
{
    float maxXAngle = 89f;
    float minXAngle = -22f;

    float maxXAngle_F = 89f;
    float minXAngle_F = -89f;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        float mouseDeltaY = -Input.GetAxis("Mouse Y"); //마우스가 위로가면 +(아래쪽)을 보게되니까 -(위쪽)을 보게함
        float currentX = transform.localEulerAngles.x;
        //Debug.Log(currentX);
        
        if (currentX > 90f) //화면이 뒤로 뒤집히는거 방지
        {
            currentX -= 360f;
            // currentX 값은 플레이어 정면기준 -90 ~ 90 까지 180의 크기만큼만 움직여아함
            // 회전값에 제한을둠 89, -22 -> 정면을 기준으로 시계반대방향으로+ 
            // 만약 정면을 기준으로 위로간다면 정면을 기준으로 시계방향으로- 
            // 아래를 보고있으면 +방향으로 

        }
        float newX = currentX + mouseDeltaY;
        //Debug.Log(newX);
        if (CamMove.FPSviewMode == true)
        {
            
            newX = Mathf.Clamp(newX, minXAngle_F, maxXAngle_F);
        }
        else if(CamMove.FPSviewMode == false)
        {
            newX = Mathf.Clamp(newX, minXAngle, maxXAngle);
        }
       
        transform.localRotation = Quaternion.Euler(newX, transform.localEulerAngles.y, 0);
        //Debug.Log(Input.GetAxis("Mouse Y"));
    }
}
