using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    ItemManager itemManager;
    GameManager gameManager;
    public static bool KeyF = false;
    public static bool MouseL = false;
    public static bool MouseR = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) //상호작용키 F
        {
            KeyF = true;
        }
        else KeyF = false;


        if (Input.GetMouseButton(0)) //좌클릭
        {
            MouseL = true;
        }
        else MouseL = false;

        if(Input.GetMouseButton(1)) //우클릭
        {
            MouseR = true;
        }
        else MouseR = false;
    }
}
