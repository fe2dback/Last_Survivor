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
        
        if (Input.GetMouseButton(0) && PlayerInput.Rifle_FireReady == true  && PlayerInput.isReload == false)
        {
            if (RifleAmmo >= 1)
            {
                if (Time.time > BeforeTime + 0.2f)
                {
                    RifleAmmo -= 1;
                    Debug.Log(RifleAmmo);
                    
                    StartCoroutine(ff());
                    Fire.Play();
                    
                    GameObject B = Instantiate(Bullet, FirePos.position, FirePos.rotation);
                    Destroy(B, 3f);


                    BeforeTime = Time.time;
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

    IEnumerator ff()
    {
        FireEffect.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        FireEffect.SetActive(false);
    }

}
