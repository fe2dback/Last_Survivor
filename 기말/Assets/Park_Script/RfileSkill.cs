using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RfileSkill : MonoBehaviour
{
    float QTime = 1f;
    float ETime = 2f;
    float RTime = 20f;

    bool AvailableQ = true;
    bool AvailableE = true;
    bool AvailableR = true;

    SkillType CurrentSkill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    enum SkillType
    {
        None,
        Q,
        E,
        R
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Delay(QTime, SkillType.Q));

        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Delay(ETime, SkillType.E));
        }
    }




    IEnumerator Delay(float Times, SkillType Skill)
    {   
        if(CurrentSkill == SkillType.None)
        {
            CurrentSkill = Skill;
            Debug.Log("스킬사용");

            switch (Skill)
            {

            }


            yield return new WaitForSeconds(Times);
            Debug.Log("스킬끝");
            CurrentSkill = SkillType.None;
        }
        else
        {
            Debug.Log("스킬사용중");
        }
        

    }

    void Skill_Q()
    {
        Gun.Delay = 0.1f;
    }

    void Skill_E()
    {
        StartCoroutine(Delay(QTime, CurrentSkill));
    }

    void Skill_R()
    {

    }

}
