using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bullet;
    public Transform FirePos;
    public GameObject FireEffect;

    int RifleAmmo = 30;
    float BeforeTime;

    public static float Delay;
    public AudioSource Fire_A;
    public AudioSource Reload_A;
    public AudioSource NoAmmo_A;


    void Start()
    {
        Delay = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0) && PlayerInput.Rifle_FireReady == true && PlayerInput.isReload == false)
        {
            if (RifleAmmo >= 1)
            {
                if (Time.time > BeforeTime + Delay)
                {
                    RifleAmmo -= 1;
                    Debug.Log(RifleAmmo);
                    
                    StartCoroutine(MuzzleFlash());
                    Fire_A.Play();
                    
                    GameObject B = Instantiate(Bullet, FirePos.position, FirePos.rotation);
                    Destroy(B, 3f);


                    BeforeTime = Time.time;
                }
            }
            else if(NoAmmo_A.isPlaying == false) 
            {
                NoAmmo_A.Play();
            }


        }
        

    }

    public void Reload()
    {
        RifleAmmo = 30;
    }


    IEnumerator MuzzleFlash()
    {
        FireEffect.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        FireEffect.SetActive(false);
    }

}
