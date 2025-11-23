using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    [Header("체력 수치")]
    public int maxHP = 100;
    private int currentHP;
    //public float CurrentHP => currentHP;

    public float getHealth()
    {
        return currentHP;
    }
    void setHealth(int health)
    {
        currentHP = health;
    }

    void Awake() // 또는 Start()
    {
        currentHP = maxHP; // 시작 시 최대 체력으로 초기화
        Debug.Log("플레이어 초기 체력: " + currentHP);
    }


    void Start()
    {
        
    }

    /*
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Bleeding(10, 3, 3);
        }
    }
    */

    // 데미지를 입는 함수
    public void TakeDamage(int damage) // 데미지도 float으로 받을 수 있도록 수정
    {
        currentHP -= damage;
        Debug.Log($"플레이어 {damage} 데미지 받음! 현재 체력: {currentHP}");

        if (currentHP <= 0)
        {
            currentHP = 0; // 체력이 0보다 낮아지지 않도록
            Debug.Log("Die()함수 실행");
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다!");
        gameObject.SetActive(false); 
    }

    /*
    void Hit(int damage)
    {
        setHealth(getHealth() - damage);
    }

    void Heal(int hp)
    {
        setHealth(getHealth() + hp);
    }
    


    void Bleeding(int damage, int bleedingdamge, int term) 
    {
        

    }
    */

}
