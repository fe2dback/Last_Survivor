using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bullet;
    public Transform FirePos;
    public GameObject FireEffect;

    public static int RifleAmmo = 0;
    int Mag = 0;
    int MaxMag = 30;
    float BeforeTime;
    public AudioSource Fire_A;
    public AudioSource Reload_A;
    public AudioSource NoAmmo_A;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0) && PlayerInput.Rifle_FireReady == true && PlayerInput.isReload == false)
        {
            if (Mag >= 1)
            {
                if (Time.time > BeforeTime + 0.2f)
                {
                    Mag -= 1;

                    Debug.Log(Mag + "/" + RifleAmmo); // ¿Â¿¸µ»≈∫ / ≥≤¿∫≈∫
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
        if(Mag == MaxMag)
        {
            Debug.Log("max");
        }

        int BulletNeed = MaxMag -  Mag;
        if (RifleAmmo < BulletNeed)
        {
            Mag += RifleAmmo;
            RifleAmmo = 0;
        }
        else
        {
            Mag = MaxMag;
            RifleAmmo -= BulletNeed;
        }
        
    }


    IEnumerator MuzzleFlash()
    {
        FireEffect.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        FireEffect.SetActive(false);
    }

}
