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


    public static bool KeyF = false;
    public static bool MouseL = false;
    public static bool MouseR = false;
    public Rig AimRig;
    


    public Rig GunHold;
    public Rig GunCarry;

    public static bool Rifle_LowReady = false;
    public static bool Rifle_FireReady = false;
    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) //상호작용키 F
        {
            KeyF = true;
        }
        else KeyF = false;


        if (Input.GetMouseButtonDown(0)) //좌클릭
        {
            
            MouseL = true;
        }
        else MouseL = false;

        if(Input.GetMouseButtonDown(1)) //우클릭
        {
            
            MouseR = true;
        }
        else MouseR = false;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Rifle_LowReady = !Rifle_LowReady;

        }

        if (Rifle_LowReady == false)
        {
            GunCarry.weight -= Time.deltaTime / 0.3f; ;
            GunHold.weight -= Time.deltaTime / 0.3f; ;
        }
        else
        {
            GunCarry.weight += Time.deltaTime / 0.3f; ;
            GunHold.weight += Time.deltaTime / 0.3f; ;
        }


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
}
