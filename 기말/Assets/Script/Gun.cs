using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bullet;
    public Transform FirePos;
    float prevT;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && PlayerInput.Rifle_FireReady == true)
        {
            
            if (Time.time > prevT + 0.2f)
            {
                audio.Play();
                GameObject B = Instantiate(Bullet, FirePos.position, FirePos.rotation);
                Destroy(B, 3f);
                prevT = Time.time;
            }
            
        }
    }

}
