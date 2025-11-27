using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    bool[] Type = new bool[3];
    State state = State.none;

    enum State
    { 
        Q,
        E,
        R,
        none
    }


    void Start()
    {
        
        for(int i = 0; i < Type.Length; i++)
        {
            
            Type[i] = true; // 초기화
        }

    }

    // Update is called once per frame
    void Update()
    {
        StateMach();
        if (Input.GetKeyDown(KeyCode.Q) && Type[0] == true)
        {    
            state = State.Q;
        }
        else if (Input.GetKeyDown(KeyCode.E) && Type[1] == true)
        {
            state = State.E;
        }
        else if(Input.GetKeyDown(KeyCode.R) && Type[2] == true)
        {
            //state = State.R; 재장전이랑 중복됨 
        }
        else
        {     
            state = State.none;
        }
    }

    void StateMach()
    {
        switch (state)
        {
            case State.Q:
                Q_();
                break;
            case State.E:
                E_();
                break;
            case State.R:
                R_();
                break;
            default:
                break;
        }
    }
    
    void Q_()
    {
        Debug.Log("Q");
        StartCoroutine(CoolDown(2f, 0));
    }

    void E_()
    {
        Debug.Log("E");
        StartCoroutine(CoolDown(3f, 1));
    }

    void R_()
    {
        Debug.Log("R");
        StartCoroutine(CoolDown(5f, 2));
    }

    IEnumerator CoolDown(float time, int index)
    {
        Type[index] = false;   
        yield return new WaitForSeconds(time);
        Type[index] = true;
        Debug.Log("end");
    }

}
