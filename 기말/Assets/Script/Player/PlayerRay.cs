using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerRay : MonoBehaviour
{

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
            if (hitObject.collider.gameObject.tag == "box" && Input.GetMouseButton(0))
            {
                
                objects.transform.position = getRay.position + getRay.forward * 3; // 충돌한 오브젝트의 위치를 보는방향 앞으로 이동함 #현재는 바로이동하지만 천천히 이동시키려면 translate 사용? or lerp
                float objsize = objects.transform.localScale.y/2; // 오브젝트의 사이즈의 절반위치가 0 + y높이 하면 물체 바닥이 0 에 맞게됨 바닥으로 떨어지는거 방지
                if (objects.transform.position.y >= objsize)
                {
                    //objects_.isKinematic = true;
                    objects_.constraints = RigidbodyConstraints.FreezeRotation;
                    
                }
                else
                {
                    objects_.transform.position = new Vector3(objects_.transform.position.x, objsize, objects_.transform.position.z);
                }
            }
            else
            {
                objects_.constraints = RigidbodyConstraints.None;
                //.isKinematic = false;
            }
            
           


            // 아이템 획득
            if (hitObject.collider.gameObject.tag == "item" && Input.GetKeyDown(KeyCode.F))
            {
                string itemTag = "";
                itemTag = hitObject.collider.gameObject.name;
                ItemManager.itemManager.getItem(itemTag, 1);
                Destroy(objects);
            }

            if(hitObject.collider.gameObject.tag == "gun" && Input.GetKeyDown(KeyCode.F))
            {
                ItemManager.Instance.HasGun = true;
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
            Debug.DrawRay(getRay.position, transform.forward * 1, Color.red);
        }
        else if (RayNum == 2)
        {
            Debug.DrawRay(getRay.position, transform.forward * 2, Color.green);
        }
        else if (RayNum == 3)
        {
            Debug.DrawRay(getRay.position, transform.forward * 4, Color.yellow);
        }
        else if (RayNum == 4)
        {
            Debug.DrawRay(getRay.position, transform.forward * 8, Color.blue);
        }
    }


}
