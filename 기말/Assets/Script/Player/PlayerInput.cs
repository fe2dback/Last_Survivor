using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class PlayerInput : MonoBehaviour
{
    ItemManager itemManager;
    GameManager gameManager;
    Animator animator;
    

    public GameObject Laser;
    public GameObject Gun;

   
    public Rig AimRig;
    public Rig Hand;
    public Rig WeaponPose;
    public TwoBoneIKConstraint LeftHand;


    public static bool isReload = false;
    float ReloadTime = 3f;
    bool Rifle_LowReady = false;
    public static bool Rifle_FireReady = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        AimRifle();
        OnRifle();
        ReLoad();

    }


    void AimRifle()
    {
        if (Input.GetMouseButton(1) && Rifle_LowReady == true)
        {
            Laser.SetActive(true);
            Rifle_FireReady = true;
            AimRig.weight += Time.deltaTime / 0.3f;
        }
        else
        {
            Laser.SetActive(false);
            Rifle_FireReady = false;
            AimRig.weight -= Time.deltaTime / 0.3f;
        }
    }
    void OnRifle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Rifle_LowReady = !Rifle_LowReady;

        }

        if (Rifle_LowReady == true)
        {
            WeaponPose.weight += Time.deltaTime / 0.3f; 
            Hand.weight += Time.deltaTime / 0.3f; 
        }
        else
        {
            WeaponPose.weight -= Time.deltaTime / 0.3f; 
            Hand.weight -= Time.deltaTime / 0.3f; 
        }
    }

    void ReLoad()
    {
        if (Input.GetKeyDown(KeyCode.R) && Rifle_LowReady== true && isReload ==false)
        {
            isReload = true;
            LeftHand.weight = 0.5f;

            
            animator.SetTrigger("Reload");
            StartCoroutine(OnReLoad());
        }
    }

    IEnumerator OnReLoad()
    {
        Gun.GetComponent<Gun>().Reload.Play();
        yield return new WaitForSeconds(ReloadTime);
        isReload = false;   
        LeftHand.weight = 1;
    }
}
