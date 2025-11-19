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
    


    void Bleeding(int damage, int bleedingdamge, int term) 
    {

        if(damage == 1)
        {
            Debug.Log("1");
        }
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("test2");
        }
    }


}
