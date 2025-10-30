using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamMove : MonoBehaviour
{
    float maxXAngle = 89f;
    float minXAngle = 0f;

    float maxXAngle_F = 89f;
    float minXAngle_F = -89f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mouseDeltaY = -Input.GetAxis("Mouse Y");
        //transform.Rotate(-1 * Input.GetAxis("Mouse Y"), 0, 0);
        float currentX = transform.localEulerAngles.x;

        // X축 각도가 180도보다 크면 음수 값으로 변환해줘요. (예: 270도 -> -90도)
        // 이렇게 해야 '0도'를 기준으로 '위쪽(-)'과 '아래쪽(+)' 회전을 정확히 계산할 수 있어.
        if (currentX > 180f)
        {
            currentX -= 360f;
        }
        float newX = currentX + mouseDeltaY;
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
