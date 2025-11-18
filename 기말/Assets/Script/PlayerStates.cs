using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    private int Heath = 100;
    
    int getHealth()
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Bleeding(10, 3, 3);
        }
    }

    void Hit(int damage)
    {
        setHealth(getHealth() - damage);
    }

    void Heal(int hp)
    {
        setHealth(getHealth() + hp);
    }
    

    private void OnCollisionEnter(Collision collision, int damage)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            Hit(damage);
        }
    }

    void Bleeding(int damage, int bleedingdamge, int term) //������������ ��������, ���ӽð�
    {
        if (Input.GetMouseButton(0) && PlayerInput.Rifle_FireReady == true)
        {

            if (Time.time > term + 0.2f)
            {
                GetComponent<AudioSource>().Play();
                GameObject B = Instantiate(Bullet, FirePos.position, FirePos.rotation);
                Destroy(B, 3f);
                prevT = Time.time;
            }

        }

    }


}
