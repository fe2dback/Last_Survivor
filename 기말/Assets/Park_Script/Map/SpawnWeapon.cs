using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWeapon : MonoBehaviour
{

    public GameObject gun;
    public Transform[] point;

    public float time = 10f;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator test()
    {
        while (true)
        {
            for (int i = 0; i < point.Length; i++)
            {
                GameObject S = Instantiate(gun, point[i].position, point[i].rotation);
                Destroy(S, time - 0.1f);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
