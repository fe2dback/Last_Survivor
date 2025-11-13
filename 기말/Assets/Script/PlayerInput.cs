using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerInput : MonoBehaviour
{
    ItemManager itemManager;
    GameManager gameManager;
    Animator animator;

    public static bool KeyF = false;
    public static bool MouseL = false;
    public static bool MouseR = false;
    public Rig rig;
    public static bool Hand_Rifle = false;
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




        if(Input.GetKeyDown(KeyCode.R) && Hand_Rifle == true)
        {
            animator.SetTrigger("Reload");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Hand_Rifle = !Hand_Rifle;
        }
        if (Hand_Rifle == true)
        {
            animator.SetBool("onRifle", true);
            if (Input.GetMouseButton(0)) { animator.SetTrigger("Fire");}
            rig.weight = 1;
        }
        else
        {
            animator.SetBool("onRifle", false);

            rig.weight = 0;
        }
    }
}
