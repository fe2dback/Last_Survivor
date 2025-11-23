using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    private int Heath = 100;

    Animator animator;

    bool check = true;
    public int getHealth()
    {
        return Heath;
    }
    void setHealth(int health)
    {
        Heath = health;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        CheckPlayer();
    }

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

    void CheckPlayer()
    {
        if (check)
        {
            if (getHealth() <= 0 || GameManager.Instance.PlayerDead == true)
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