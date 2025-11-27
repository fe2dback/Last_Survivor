using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;


public class PlayerInput : MonoBehaviour
{
    
    Animator animator;

    public GameObject Laser;
    public GameObject Gun;
    public GameObject Knife;
    public GameObject KnifeArea;


    public Rig AimRig;
    public Rig Hand;
    public Rig WeaponPose;
    public Rig Head;
    public TwoBoneIKConstraint LeftHand;


    bool CansSwitch = true;


    //라이플
    public static bool isReload = false;
    public static float ReloadTime = 3f;
    bool Rifle_LowReady = false;
    public static bool Rifle_FireReady = false;

    //나이프
    bool Knife_Ready = false;
    public bool Knife_Attack = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        
        
        if (CansSwitch && !GameManager.Instance.PlayerDead)
        {
            if (ItemManager.Instance.HasGun == true)
            {
                Gun.SetActive(true);
                OnRifle();
            }
            KnifeAttack();
        }
        else
        {
            Head.weight = 0;
        }
        


        if (Rifle_LowReady == true)
        {
            Knife_Ready = false;
            Knife.SetActive(false);
        }
        else
        {
            Knife_Ready = true;
            animator.SetTrigger("EndReload");
            Gun.GetComponent<Gun>().Reload_A.Stop(); // 사운드정지
            LeftHand.weight = 1; //왼손 다시빠꾸
            Knife.SetActive(true);
        }


    }








    IEnumerator KnifeDelay()
    {
        yield return new WaitForSeconds(0.5f);
        KnifeArea.SetActive(true);
        Knife_Attack = true;
        yield return new WaitForSeconds(0.5f);
        Knife_Attack = false;
        KnifeArea.SetActive(false);

    }
    void KnifeAttack()
    {
        if(Knife_Ready)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                animator.SetInteger("Attack", 1); 
                StartCoroutine(KnifeDelay());

            }
            else if (Input.GetMouseButtonDown(1))
            {
                
                animator.SetInteger("Attack", 2);
                StartCoroutine(KnifeDelay());
            }
            else
            {
                
                animator.SetInteger("Attack", 0);
            }
        }
        
    }

    
    

    void AimRifle()
    {
        
        if (Input.GetMouseButton(1) && Rifle_LowReady == true && !isReload)
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
        AimRifle();
        ReLoad();
    }

    void ReLoad()
    {
        if (Input.GetKeyDown(KeyCode.R) && Rifle_LowReady == true && isReload == false)
        {
            isReload = true;
            //CansSwitch = false;
            LeftHand.weight = 0.5f;
            animator.SetTrigger("Reload");
            StartCoroutine(OnReLoad());
            
        }
    }

    IEnumerator OnReLoad()
    {      
        Gun.GetComponent<Gun>().Reload_A.Play();   
        yield return new WaitForSeconds(ReloadTime);        
        Gun.GetComponent<Gun>().Reload();
        isReload = false;
        CansSwitch = true;
        LeftHand.weight = 1;
    }

}
