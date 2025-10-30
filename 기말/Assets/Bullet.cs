using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody bullets;
    // Start is called before the first frame update
    void Start()
    {
        bullets = GetComponent<Rigidbody>();
        bullets.AddForce(transform.forward * 1000f);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
