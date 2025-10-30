using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody bullet;

    // Start is called before the first frame update
    void Start()
    {

        bullet = GetComponent<Rigidbody>();
        bullet.AddForce(transform.forward * 1000f);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
