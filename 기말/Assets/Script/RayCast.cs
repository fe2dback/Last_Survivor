using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RayCast : MonoBehaviour
{
    RaycastHit hit;

    GameObject ob;
    public Transform getRay;
    Vector3 pos;
    float maxDistance = 2f;
    itemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(getRay.position, getRay.forward * 10f, Color.red);
        //Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.yellow);
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootRay();
        }
        else if(Input.GetMouseButton(0))
        {
            playerHand();
        }

    }

    void playerHand()
    {
        
        if (Physics.Raycast(getRay.position, getRay.forward, out hit, 15f))
        {
            pos = hit.transform.position;
            if (hit.collider.gameObject.tag == "box" && Input.GetMouseButton(0))
            {
                ob = hit.collider.gameObject;

                ob.transform.position = getRay.position + getRay.forward * 4f;
                Debug.Log("box hit");

            }

            //Debug.Log(hit.collider.gameObject.name.ToString());
        }
    }

    void ShootRay()
    {
       
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            
           if(LayerMask.LayerToName(hit.collider.gameObject.layer) == "item")
            {
                //Debug.Log(hit.collider.gameObject.name);
                itemManager.getItem("구급상자");

                Destroy(hit.collider.gameObject);
                //블럭형 아이템을 들고 옮길 수 있게 구현할것
            }
        }
    }
}
