using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    Rigidbody bullets;
    public float bulletspeed;
    // Start is called before the first frame update
    void Start()
    {
        bullets = GetComponent<Rigidbody>();
        bullets.AddForce(transform.forward * bulletspeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
