using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody bullets;
    public float bulletspeed;
    public float bulletDamage = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        bullets = GetComponent<Rigidbody>();
        bullets.AddForce(transform.forward * bulletspeed);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2f);
    }

   
}
