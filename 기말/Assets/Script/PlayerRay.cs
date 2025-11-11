using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerRay : MonoBehaviour
{
    ItemManager itemManager;

    RaycastHit hitObject;
    GameObject objects; //캐스팅된 오브젝트
    Rigidbody objects_;
    public Transform getRay; // 레이캐스트 위치
 
    //testfield
    int RayNum;

    private void Start()
    {

    }

    void Update()
    {
        drawRay();
        playerRay();
        

    }

    void playerRay()
    {
        
        if (Physics.Raycast(getRay.position, getRay.forward, out hitObject, RayNum) 
            && hitObject.rigidbody != null) 
            // Raycast의 충돌을 받음 만약 충돌한 오브젝트에 Rigidbody가없으면 X
            // -> 벽과 바닥에 충돌하는 문제로 일단 둠
        {
                // 물체를 들고 옮기기

                objects = hitObject.collider.gameObject; //충돌한 오브젝트의 게임오브젝트를 담음
                objects_ = objects.GetComponent<Rigidbody>(); // 충돌한 오브젝트의 RB를 받음
                if (hitObject.collider.gameObject.tag == "box" && PlayerInput.MouseL)
                {

                    objects.transform.position = getRay.position + getRay.forward * 3; // 충돌한 오브젝트의 위치를 보는방향 앞으로 이동함 #현재는 바로이동하지만 천천히 이동시키려면 translate 사용? or lerp
                    objects_.isKinematic = true;
                }
                else
                {
                    objects_.isKinematic = false;
                }



                // 아이템 획득
                if (hitObject.collider.gameObject.tag == "item" && PlayerInput.KeyF)
                {
                    string itemTag = "";
                    itemTag = hitObject.collider.gameObject.name;
                    ItemManager.itemManager.getItem(itemTag, 1);
                    Destroy(objects);
                }
            
        }
    }

    void drawRay()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RayNum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RayNum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RayNum = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RayNum = 4;
        }


        if (RayNum == 1)
        {
            Debug.DrawRay(transform.position, transform.forward * 1, Color.red);
        }
        else if (RayNum == 2)
        {
            Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
        }
        else if (RayNum == 3)
        {
            Debug.DrawRay(transform.position, transform.forward * 4, Color.yellow);
        }
        else if (RayNum == 4)
        {
            Debug.DrawRay(transform.position, transform.forward * 8, Color.blue);
        }
    }

    
}
