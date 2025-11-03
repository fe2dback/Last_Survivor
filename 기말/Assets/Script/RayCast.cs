using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    RaycastHit hit;
    float maxDistance = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.yellow);
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShootRay();
        }
        
    }

    void ShootRay()
    {
       
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            
           if(LayerMask.LayerToName(hit.collider.gameObject.layer) == "item")
            {
                Debug.Log("아이템 획득!");
                Destroy(hit.collider.gameObject);
                //블럭형 아이템을 들고 옮길 수 있게 구현할것
            }
        }
    }
}
