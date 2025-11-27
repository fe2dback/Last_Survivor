using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStates : MonoBehaviour
{
    [Header("UI연결")]
    public Image hpFill;
    public Image expFill;

    [Header("UI 수치")]
    public int level = 1;
    public TMP_Text levelText;
    public float maxHP = 100f;
    private float currentHP;
    public float maxEXP = 100f; 
    private float currentEXP = 0f;
    

    Animator animator;

    bool check = true;
    public void ApplyStatChanges()
    {
        
        var upgrade = PlayerStatsUpgrade.Instance;

        
        maxHP += upgrade.hpBonus;
        currentHP += upgrade.hpBonus;


        UpdateHPBar();
        Debug.Log($"스탯 적용됨 → HP:{maxHP}");
    }
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
        maxHP = 100f;
        currentHP = maxHP;
        UpdateHPBar();
        currentEXP = 0f;
        UpdateEXPBar();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //체력 감소, 경험치 증가 테스트용 함수
        if (Input.GetKeyDown(KeyCode.T))
        {
            Hit(4);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GainExp(16);
        }
        CheckPlayer();
    }
   
    //체력, 경험치 이미지 갱신
    private void UpdateHPBar()
    {
        hpFill.fillAmount = currentHP / maxHP;
    }
    private void UpdateEXPBar()
    {
        expFill.fillAmount = currentEXP / maxEXP;
    }

    //경험치 획득
    public void GainExp(float amount)
    {
        currentEXP += amount;
        
        if (currentEXP >= maxEXP)
        {
            currentEXP -= maxEXP;
            level++;
            maxEXP *= 1.3f; // 다음 레벨 요구치 상승


            LevelUpHandler.Instance.OnLevelUp(level); // 레벨업 ui
        }
        UpdateLevelUI();
        UpdateEXPBar();
    }
    //레벨 UI
    private void UpdateLevelUI()
    {
        levelText.text = $": {level}";
    }

    //피격
    public void Hit(float damage)
    {
        setHealth(getHealth() - damage);
        Debug.Log("공격받음 현재 체력: " + currentHP);
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