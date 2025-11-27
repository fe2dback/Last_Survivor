using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{

    public GameObject gun;
    public Transform[] point;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i <= point.Length; i++)
        {
            Instantiate(gun, point[i].position, point[i].rotation);
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
