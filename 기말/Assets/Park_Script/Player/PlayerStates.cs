using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStates : MonoBehaviour
{
    [Header("UI연결")]
    public Image hpFill;

    [Header("체력 수치")]
    public float maxHP = 100f;
    private float currentHP;

    Animator animator;

    bool check = true;
    public float getHealth()
    {
        return currentHP;
    }
    void setHealth(float health)
    {
        currentHP = health;
    }
    // Start is called before the first frame update
    void Start()
    {
        //체력 초기화 , 나중에 묶어서 메서드 만들거임
        currentHP = maxHP;
        UpdateHPBar();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //체력 감소 테스트용 함수
        if (Input.GetKeyDown(KeyCode.T))
        {
            Hit(4);
        }
        CheckPlayer();
    }

    //체력 이미지 갱신
    private void UpdateHPBar()
    {
        hpFill.fillAmount = currentHP / maxHP;
    }

    //피격
    void Hit(float damage)
    {
        setHealth(getHealth() - damage);
        //변경사항
        UpdateHPBar();
    }

    void Heal(int hp)
    {
        setHealth(getHealth() + hp);
    }

    void Bleeding(int damage, int bleedingdamge, int term)
    {

    }
    //GameManager, ItemManger 부분 Instance 에러로 인해 비활성화
    
    void CheckPlayer()
    {
        if (check)
        {
            if (getHealth() <= 0)
            {
                GameManager.Instance.PlayerDead = true;
                ItemManager.Instance.HasGun = false;
                animator.SetTrigger("isDead");

                check = false;
            }
            else
            {
                return;
            }
        }

    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Hit(75);
            Destroy(other.gameObject);
            //Debug.Log(getHealth());
        }

    }



}