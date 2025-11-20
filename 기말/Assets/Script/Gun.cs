using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bullet;
    public Transform FirePos;

    int RifleAmmo = 30;
    float prevT;
    public AudioSource Fire;
    public AudioSource Reload;
    public AudioSource NoAmmo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(RifleAmmo);
        if (Input.GetMouseButton(0) && PlayerInput.Rifle_FireReady == true  && PlayerInput.isReload == false)
        {
            if (RifleAmmo >= 1)
            {
                if (Time.time > prevT + 0.2f)
                {
                    Fire.Play();
                    GameObject B = Instantiate(Bullet, FirePos.position, FirePos.rotation);
                    RifleAmmo -= 1;
                    Destroy(B, 3f);
                    prevT = Time.time;
                }
            }
            else if(NoAmmo.isPlaying == false) 
            {
                NoAmmo.Play();
            }


        }

        if (PlayerInput.isReload == true)
        {
            RifleAmmo = 30;
        }

    }

}
