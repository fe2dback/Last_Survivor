using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemManager : MonoBehaviour
{

    private GameObject[] inventory = new GameObject[5]; //인벤토리 6칸
    private int[] value = new int[5];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            showItem();
        }
    }

    public void getItem(string itemTag)
    {
        if (itemTag == "구급상자")
        {
            value[0] += 1;

        }


    }


    void showItem()
    {
        Debug.Log(value[0]);
    }
}
