using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    PlayerInput playerInput;
    public float KnifeDamage = 25f;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy") && playerInput.Knife_Attack)
        {
            Debug.Log("knife hit");
        }
    }*/
}
