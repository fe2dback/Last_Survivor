using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    private float exp;
    private int level;


    private int hp;
    private float mp; //스테미나 or 마나
    
    

    private float leftTime = 30f;

    private int attack; //공격력
    private int def; // 방어력


    void Start()
    {
        StartCoroutine(timeDecrease());
    }


    private void Update()
    {
        
    }



    IEnumerator timeDecrease()
    {
        while (true)
        {
            leftTime -= Time.deltaTime;
            if(leftTime <= 0)
            {
                //게임오버 처리
                Debug.Log("게임오버");
                break;
            }
            yield return null;
        }
    }






    int getLevel()
    {
        return level;
    }

    void setLevel(int get)
    {
        this.level += get;
    }






}
