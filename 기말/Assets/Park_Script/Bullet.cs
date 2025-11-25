using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    Rigidbody bullets;
    public float bulletspeed;
    public float bulletDamage = 10.0f; //bullet데미지 추가 -민
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

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.gameObject);
        
    }
}
